using BusinessLogic;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.ResponseModels;
using System;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class TokenController(TokenBLL tokenBLL) : ControllerBase
{
    private readonly TokenBLL _tokenBLL = tokenBLL;
 

    [HttpGet]
    [Route("generate")]
    [Authorize(Roles = Roles.UniversityAdmin)]
    public IActionResult GenerateToken()
    {
        string token = Guid.NewGuid().ToString();
        DateTime expirationDate = DateTime.UtcNow.AddDays(7);

        return Ok(_tokenBLL.saveToken(token, expirationDate));
    }

    [HttpGet]
    [Route("validate/{token}")]
    public IActionResult ValidateToken(string token)
    {
        if (!_tokenBLL.isTokenValid(token))
        {
            return Ok(new TokenResponse
            {
                isTokenExpired = true,
            });
        }
        else
        {
            return Ok(new TokenResponse { isTokenExpired = false});
        }
    }
}
