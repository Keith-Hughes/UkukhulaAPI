using DataAccess.Entity;
using DataAccess;
using BusinessLogic.Models.Response;
using System.Transactions;
using Shared.Models;

namespace BusinessLogic
{
    public class StudentFundRequestBLL
    {
        private readonly StudentFundRequestDAL _repository;
        private readonly UserDAL _userDAL;
        public StudentFundRequestBLL(StudentFundRequestDAL repository, UserDAL userDAL)
        {
            _userDAL = userDAL;
            _repository = repository;
        }

        public IEnumerable<StudentFundRequest> GetAllRequests()
        {
            try
            {
                return (IEnumerable<StudentFundRequest>)_repository.GetAllRequests();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving student fund requests: {ex.Message}");
            }
        }

        public UserManagerResponse Create(CreateStudentFundRequestForNewStudent newRequest)
        {
            if (newRequest != null)


                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        Models.Register userRequest = new()
                        {

                            FirstName = newRequest.FirstName,
                            LastName = newRequest.LastName,
                            Email = newRequest.Email,
                            PhoneNumber = newRequest.PhoneNumber,
                            Role = "Student",
                            

                        };
                     
                       
                            //Create Contacts object and insert to table,
                            ContactDetails contactDetails = new ContactDetails
                            {
                                Email = newRequest.Email,
                                PhoneNumber = newRequest.PhoneNumber,
                            };
                            int contactId = _userDAL.InsertContactsAndGetPrimaryKey(contactDetails);

                            //create User object and insert User
                            User user = new User
                            {
                                FirstName = newRequest.FirstName,
                                LastName = newRequest.LastName,
                                ContactID = contactId,
                            };
                            int userId = _userDAL.InsertUserAndGetPrimaryKey(user);

                            //insert UserRole Reference
                            _userDAL.InsertToUserRole(userId, userRequest.Role);




                            // Convert the business logic model to the data access model
                            CreateStudentFundRequestForNewStudent dataAccessModel = new()
                            {
                                IDNumber = newRequest.IDNumber,
                                FirstName = newRequest.FirstName,
                                LastName = newRequest.LastName,
                                Email = newRequest.Email,
                                PhoneNumber = newRequest.PhoneNumber,
                                GenderName = newRequest.GenderName,
                                RaceName = newRequest.RaceName,
                                UniversityID = newRequest.UniversityID,
                                BirthDate = newRequest.BirthDate,
                                Grade = newRequest.Grade,
                                Amount = newRequest.Amount,
                                UserID = userId

                            };

                            _repository.Create(dataAccessModel);

                            scope.Complete();
                            return new UserManagerResponse
                            {
                                UniversityID = newRequest.UniversityID,
                                Message = "Successfully Submited Student Request For "+newRequest.FirstName,
                                isSuccess = true,
                            };

                        
                    }
                }

                catch (Exception ex)
                {
                    return new UserManagerResponse
                    {
                        UniversityID = newRequest.UniversityID,
                        Message = "Error creating student fund request" + ex.Message,
                        isSuccess = false,
                    };
                    throw new Exception("Error creating student fund request" + ex.Message);
                }

            else
                return new UserManagerResponse
                {
                    UniversityID = newRequest.UniversityID,
                    Message = new ArgumentNullException(nameof(newRequest)).Message,
                    isSuccess = false,
                };
            
        }

        public IEnumerable<StudentFundRequest> GetStudentFundRequestsByUniversity(int universityID)
        {
            try
            {
                return _repository.GetStudentFundRequestsByUniversity(universityID);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving student fund requests: {ex.Message}");
            }
        }
        public void CreateForExistingStudent(Models.ExistingStudent newRequest)
        {
            if (newRequest != null)
                try
                {
                    ExistingStudent dataAccessModel = new()
                    {
                        StudentID = newRequest.StudentID,
                        Grade = newRequest.Grade,
                        Amount = newRequest.Amount
                    };

                    _repository.CreateForExistingStudent(dataAccessModel);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating student fund request" + ex.StackTrace);
                }
            else
                throw new ArgumentNullException(nameof(newRequest));
        }


        public void UpdateRequest(int id, Models.UpdateStudentFundRequest newRequest)
        {
            if (newRequest == null)
                throw new ArgumentNullException(nameof(newRequest));

            try
            {
                UpdateStudentFundRequest updatedRequest = new()
                {
                    Grade = newRequest.Grade,
                    Amount = newRequest.Amount
                };
                _repository.UpdateRequest(id, updatedRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating student fund request: {ex.Message}");
            }
        }

        public StudentFundRequest ApproveApplication(int applicationId)
        {
            try
            {
                return _repository.UpdateApplicationStatus(applicationId, 1, "");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error approving application: {ex.Message}");
            }
        }

        public StudentFundRequest RejectApplication(int applicationId, string comment)
        {
            try
            {
                return _repository.UpdateApplicationStatus(applicationId, 2, comment);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error rejecting application: {ex.Message}");
            }
        }


    }
}
