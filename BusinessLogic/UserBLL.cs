
using BusinessLogic.Models;
using BusinessLogic.Models.Response;
using DataAccess;
using DataAccess.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Transactions;

namespace BusinessLogic
{
    public class UserBLL
    {
        private UserDAL _userDAL;
        private IConfiguration _configuration;
        public UserBLL(UserDAL userDAL, IConfiguration configuration)
        {
            _configuration = configuration;
            _userDAL = userDAL;
            _configuration = configuration;
        }

        public async Task<UserManagerResponse> LoginUserAsync(Login model)
        {
            var user = await _userDAL.checkUserByEmail(model.Email);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that Email Address",
                    isSuccess = false,
                };
            }

            bool result = await _userDAL.checkUserPassword(user, model.Password);

            if (result == false)
            {
                return new UserManagerResponse
                {
                    Message = "Incorrect Password",
                    isSuccess = false,
                };
            }
            
            Claim[] claims = new[]
            {
                
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
                
            };
            var roles = await _userDAL.getUserRoles(user);
            var claimsWithRoles = roles.Select(role => new Claim(ClaimTypes.Role, role));
            var allClaims = claims.Concat(claimsWithRoles);

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: allClaims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenString,
                isSuccess = true,
                ExpireDate = token.ValidTo
            };
        }

        public UserManagerResponse ProcessRegistration(Register model)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if(model.Password != model.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    Message = "Passwords do not match",
                    isSuccess = false
                };
            }

            var applicationUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                
            };
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))//Need to add option to identify async functions
            {
                try {
                    IdentityResult result = _userDAL.RegisterIdentityUser(applicationUser, model.Password, model.Role).Result;

                    if (result.Succeeded)
                    {
                        ContactDetails contacts = new ContactDetails
                        {
                            Email = model.Email,
                            PhoneNumber = model.PhoneNumber,
                        };
                        int contactId = _userDAL.InsertContactsAndGetPrimaryKey(contacts);

                        User user = new User
                        {
                            ContactID = contactId,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                        };
                        int userId = _userDAL.InsertUserAndGetPrimaryKey(user);

                        _userDAL.InsertToUserRole(userId, model.Role);




                        scope.Complete();
                        return new UserManagerResponse
                        {
                            Message = $"User created\nDetails:\nUsername: {model.Email}",
                            isSuccess = true
                        };
                    }
                    return new UserManagerResponse
                    {
                        Message = "User not created",
                        isSuccess = false,
                        Errors = result.Errors.FirstOrDefault().Description
                    };
                }
                catch (TransactionAbortedException ex)
                {

                    Console.WriteLine(ex.Message);
                    return new UserManagerResponse
                    {
                        Message = "User not created",
                        isSuccess = false,
                        Errors = ex.Message
                    };
                }

                

            }


            
        }

        public async Task<User> getUser(string email)
        {
            return _userDAL.getUserByEmail(email);

        }
    }
}
