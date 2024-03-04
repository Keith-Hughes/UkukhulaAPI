using BusinessLogic;
using BusinessLogic.Models;
using BusinessLogic.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Models;
using Shared.ResponseModels;


namespace BursaryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentFundRequestController(StudentFundRequestBLL StudentFundRequestBLL) : ControllerBase
    {
        private readonly StudentFundRequestBLL _StudentFundRequestBLL = StudentFundRequestBLL;

        [HttpGet]
        public ActionResult<IEnumerable<StudentFundRequest>> GetAllRequests()
        {
            try
            {
                var requests = _StudentFundRequestBLL.GetAllRequests();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving student fund requests: {ex.Message}");
            }
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateStudentFundRequestForNewStudent newRequest)
        {

            if (!ModelState.IsValid)
            {
                return Ok(new StudentRequestResponse
                {
                    IsSuccess = false,
                    Message = "Invalid information received: " + ModelState.ToString()
                });
            }

            try
            {
                StudentRequestResponse studentRequestResponse = _StudentFundRequestBLL.Create(newRequest);
                if (!studentRequestResponse.IsSuccess)
                {
                    return Ok(studentRequestResponse);
                }

                return Ok(studentRequestResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating student fund request: {ex.Message}");
            }
        }

        [HttpPost("ExistingStudent")]
        public ActionResult CreateForExistingStudent([FromBody] ExistingStudent newRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _StudentFundRequestBLL.CreateForExistingStudent(newRequest);
                return Ok("Student fund request created successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating student fund request: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.UniversityAdmin)]
        [HttpPut("updateRequest/{id}")]
        public ActionResult UpdateRequest(int id, [FromBody] UpdateStudentFundRequest updatedRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _StudentFundRequestBLL.UpdateRequest(id, updatedRequest);
                return Ok("Student fund request updated successfully!");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Student fund request not found!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating student fund request: {ex.Message}");
            }
        }
        [Authorize(Roles = Roles.BBDAdmin)]
        [HttpPut("{applicationId}/approve")]
        public ActionResult ApproveApplication(int applicationId)
        {
            try
            {
                StudentFundRequest request = _StudentFundRequestBLL.ApproveApplication(applicationId);
                if (request.ID == 0)
                {
                    return BadRequest(request);
                }
                else
                    return Ok(request);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error approving application: {ex.Message}");
            }
        }

        
        [Authorize(Roles = Roles.BBDAdmin+","+Roles.UniversityAdmin)]
       
        [HttpGet("get/{universityId}")]
        public ActionResult<IEnumerable<StudentFundRequest>> getByUniversity(int universityId) 
        { 
            if(ModelState.IsValid) 
            { 
                IEnumerable<StudentFundRequest> allRequests=  _StudentFundRequestBLL.GetStudentFundRequestsByUniversity(universityId);
                if (allRequests.Any())
                {
                    return Ok(allRequests);
                };
                return Ok("{Message: No Requests Found}");
            }
            return BadRequest("{Message: Unable to retrieve your requests}");
        }


        [Authorize(Roles = Roles.BBDAdmin)]
        [HttpPost("{applicationId}/reject")]
        public ActionResult RejectApplication(int applicationId, string comment)
        {
            try
            {
                StudentFundRequest request = _StudentFundRequestBLL.RejectApplication(applicationId, comment);
                if (request.ID == 0)
                {
                    return BadRequest(request);
                }
                else
                    return Ok(request);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error rejecting application: {ex.Message}");
            }
        }
    }
}