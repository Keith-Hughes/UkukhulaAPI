using BusinessLogic.Models.Response;
using DataAccess;
using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic

{
    public class AdminBLL(AdminDAL repository)
    {
        private readonly AdminDAL _repository = repository;
        public BudgetResponse getBudgetAndFunds(){
             try
            {
                BBDAllocation?bBDAllocation=  _repository.getBudgetAndFunds();
                if(bBDAllocation!= null){
                    return new BudgetResponse
                    {
                        budget = bBDAllocation.getBudget(),
                        funds = _repository.getMoneySpentForYear()
                    };
                }
                 
            }
            catch (Exception ex)
            {

                throw new Exception($"Error getting Budget and Funds left: {ex.Message}");
                
            }
            return null;
        }

        

        public IEnumerable<UniversityRequest> GetAllUniversityRequests()
        {
            try
            {
                return _repository.GetAllUniversityFundRequests();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting university requests: {ex.Message}");
            }
        }
        public void UpdateUniversityRequestStatus(int UniversityID, int StatusID)
        {
            repository.UpdateUniversityRequestStatus(UniversityID, StatusID);

        }
        public List<UniversityUser> GetAllUniversityUsers()
        {
            try
            {
                return  _repository.GetUniversityUsers();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting university Users: {ex.Message}");
            }
        }

        public IEnumerable<University> GetAllUniversityDetails() {
            try {
                return _repository.GetUniversities();
            } catch (Exception ex) {
                throw new Exception($"Error getting universities: {ex.Message}");
            }
        }

        public Dictionary<string,string> UpdateUniversityRequest(int requestId, int StatusId)
        {
            try
            {
                return _repository.UpdateUniversityFundRequest(requestId, StatusId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating university request: {ex.Message}");
            }
        }

        public UniversityRequest NewUniversityRequest(int universityID, decimal amount, string comment)
        {
            try
            {
                return _repository.NewUniversityFundRequest(universityID, amount, comment);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating new university request: {ex.Message}");
            }
        }
        public Dictionary<string,string> updateUserActivity(int UserID,string Status)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            try
            {
                
                int response = _repository.updateUserActivity(UserID,Status);
                if (response==1){
                    dictionary.Add("message", "User Updated successfully");
                    return dictionary;
                }
                dictionary.Add("message", "Failed to update user");
                return dictionary;
            }
            catch (Exception ex)
            {
                dictionary.Add("message", "Failed to update user");
                return dictionary;
            }
        }

        public IEnumerable<AllocationDetails> GetAllocationDetails()
        {
            try
            {
                return _repository.GetUniversityFundAllocations();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting allocation details: {ex.Message}");
            }
        }

        public FundsResponse Allocate()
        {
            // try
            // {
                int result = _repository.Allocate();
                if(result ==0){
                    return new FundsResponse
                    {
                        responeMessage = "There is currently no fund allocation for this year",
                        isSuccess = false
                    };
                }

                if(result ==1){
                    return new FundsResponse
                    {
                        responeMessage = "There are pending universities, make sure that all universities have been responed to for this year",
                        isSuccess = false
                    };
                }

                if(result ==2){
                    return new FundsResponse
                {
                    responeMessage= "The funds have been equally allocated successfully",
                    isSuccess = true
                };
                }

                if(result ==3){
                    return new FundsResponse
                    {
                        responeMessage = "The funds have been equally allocated but there are accepted universities that were not included ",
                        isSuccess = true
                    };
                }
                if(result ==4){
                    return new FundsResponse
                    {
                        responeMessage = "You have funded already all the approved universities for this year. ",
                        isSuccess = false
                    };
                }

                if(result ==5){
                    return new FundsResponse
                    {
                        responeMessage = "Total budget exceeds the allocated budget for the year.",
                        isSuccess = false
                    };
                }

                 return new FundsResponse
                    {
                        responeMessage ="Error allocating funds",
                        isSuccess = false
                    };
                
                
            // }
            // catch (Exception ex)
            // {
            //     throw new Exception("Error allocating funds", ex);
            // }
        }
    }
}