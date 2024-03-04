
using DataAccess;
using Shared.ResponseModels;


namespace BusinessLogic
{
    public class TokenBLL(TokenDAL tokenDAL)
    {
        private readonly TokenDAL _tokenDAL = tokenDAL;
        public TokenResponse saveToken(string token, DateTime expirationDate)
        {
            _tokenDAL.saveToDatabase(token, expirationDate);
            return new TokenResponse
            {
                Token = token,
                expiresAt = expirationDate

            };
        }

        public bool isTokenValid(string token)
        {
            Dictionary<string, DateTime> Token = _tokenDAL.getTokens(token);
            if (Token == null)
            {
                return false;
            }
            else
            {
                return Token.TryGetValue(token, out var expirationDate) && expirationDate > DateTime.UtcNow;
            }
        }

    }
}
