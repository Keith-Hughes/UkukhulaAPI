using BusinessLogic;
using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("api/[controller]")]
public class EmailController(Email email) : ControllerBase
{
    private readonly Email _email = email;
 

    [HttpPost]
    [Route("send")]
    //[Authorize(Roles = Roles.UniversityAdmin)]
    public IActionResult SendEmail(string subject, string message, string toEmail, string toName)
    {
        _email.SendMail(subject, message,toEmail,toName);

        return Ok("Email Sent");
    }

  
}
