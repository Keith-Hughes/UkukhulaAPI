using BusinessLogic;
using BusinessLogic.Models;
using BusinessLogic.Models.Response;
using DataAccess.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BursaryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(AdminBLL adminBLL) : ControllerBase
    {
        
        private readonly AdminBLL _adminBLL = adminBLL;

        [Route("GetAllUniversityRequests")]
        [HttpGet]
       // [Authorize(Roles = Roles.BBDAdmin)]
        public ActionResult<IEnumerable<UniversityRequest>> Get(int pageNumber = 1, int pageSize = 30)
        {
           
            try
            {
                return Ok(_adminBLL.GetAllUniversityRequests(pageNumber,pageSize));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Route("updateUserActivity")]
        [HttpPut]
        [Authorize(Roles = Roles.BBDAdmin)]
        public ActionResult<IEnumerable<AllocationDetails>> updateUserActivity(int UserID,string Status)
        {
            try
            {
                return Ok(_adminBLL.updateUserActivity(UserID,Status));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Route("GetUniversityAllocationsByYear")]
        [HttpGet]
        [Authorize(Roles = Roles.BBDAdmin)]
        public ActionResult<IEnumerable<AllocationDetails>> GetYearAllocations()
        {
            try
            {
                return Ok(_adminBLL.GetAllocationDetails());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("GetUniversityUsers")]
        [HttpGet]
        [Authorize(Roles = Roles.BBDAdmin)]
        public ActionResult<List<UniversityUser>> GetUniversityUsers()
        {
            try
            {
                return Ok(_adminBLL.GetAllUniversityUsers());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Route("GetAllUniversities")]
        [HttpGet]
        [Authorize(Roles = Roles.BBDAdmin)]
        public ActionResult<IEnumerable<University>> GetAllUniversities() {
            try { 
                return Ok(_adminBLL.GetAllUniversityDetails());
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        
        }
        [Route("AllocateProjection")]
        [HttpGet]
        [Authorize(Roles = Roles.BBDAdmin)]
        public List<UniversityFundAllocation> putProjection(){

            List<UniversityFundAllocation> universityFundAllocations = _adminBLL.AllocateProjection();
            // universityFundAllocations.ForEach(i => {
            //    Console.WriteLine( i.getDateAllocated());
            // });
                return universityFundAllocations;
        }

        [Route("allocateUniversityBudget")]
        [HttpPost]
        [Authorize(Roles = Roles.BBDAdmin)]
        public ActionResult PostUniversity(int URequestID,decimal AmountAllocated)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }
            // try
            // {
            int response = _adminBLL.AllocateUniversity(URequestID,AmountAllocated);
            if (response ==1)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(500, response);
            }
        }

        [Route("allocateBuget")]
        [HttpPost]
        [Authorize(Roles = Roles.BBDAdmin)]
        public ActionResult Post()
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }
            // try
            // {
                FundsResponse response = _adminBLL.Allocate();
                if (response.isSuccess){
                     return Ok(response);
                }
                else{
                   return StatusCode(500, response);
                }
               
            // }
            // catch (System.Exception ex)
            // {
            //     return StatusCode(500, new status("Unsuccessful", "Error: " + ex.Message));
            // }
        }
         [Route("getBudgetAndFunds")]
        [HttpGet]
        [Authorize(Roles = Roles.BBDAdmin)]
        public IActionResult getBudgetAndFunds(){
              try
                {
                   return Ok(_adminBLL.getBudgetAndFunds());
                }
                catch (Exception ex)
                {
                  return  BadRequest(ex.Message);
                }
        }


        [Route("newUniversityRequest")]
        [HttpPost]
        [Authorize(Roles =Roles.UniversityAdmin)]
        [Authorize(Roles = Roles.BBDAdmin)]
        public ActionResult Post(NewUniversityRequest newUniversity )
        {
            if (!ModelState.IsValid)
            {
              
                return BadRequest();
            }
            
            return Ok(newUniversity);
            
        }

        [Route("updateUniversityRequest")]
        [HttpPut]
        [Authorize(Roles = Roles.BBDAdmin)]
        public IActionResult Put(int requestId, int statusId)
        {
            if (requestId == 0 || statusId == 0)
            {
               return BadRequest("Invalid input");
            }
            else
            {
                try
                {
                   return Ok(_adminBLL.UpdateUniversityRequest(requestId, statusId));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [Route("rejectUniversityRequest")]
        [HttpPut]
        [Authorize(Roles = Roles.BBDAdmin)]
        public IActionResult RejectUniversity(int requestId, int statusId,string comment)
        {
            if (requestId == 0 || statusId == 0)
            {
                return BadRequest("Invalid input");
            }
            else
            {
                try
                {
                    return Ok(_adminBLL.RejectUniversityRequest(requestId, statusId, comment));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
