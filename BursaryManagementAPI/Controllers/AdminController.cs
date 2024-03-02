using BusinessLogic;
using BusinessLogic.Models;
using DataAccess.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<UniversityRequest>> Get()
        {
           
            try
            {
                return Ok(_adminBLL.GetAllUniversityRequests());
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
        public ActionResult<IEnumerable<AllocationDetails>> GetUniversityUsers()
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

        [Route("allocateBuget")]
        [HttpPost]
        [Authorize(Roles = Roles.BBDAdmin)]
        public ActionResult Post()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _adminBLL.Allocate();
                return Ok(new status("Succesful", "Budget allocated in the all the institutions"));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new status("Unsuccessful", "Error: " + ex.Message));
            }
        }

        [Route("newUniversityRequest")]
        [HttpPost]
        [Authorize(Roles =Roles.UniversityAdmin)]
        [Authorize(Roles = Roles.BBDAdmin)]
        public void Post(int universityID, decimal amount, string comment)
        {
            if (universityID == 0 || amount == 0 || comment == null)
            {
                BadRequest("Invalid input");
            }
            else
            {
                try
                {
                    Ok(_adminBLL.NewUniversityRequest(universityID, amount, comment));
                }
                catch (Exception ex)
                {
                    BadRequest(ex.Message);
                }
            }
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
    }
}