using BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

[ApiController]
[Route("[controller]")]
public class TokenController : ControllerBase
{
    private static readonly Dictionary<string, DateTime> Tokens = new Dictionary<string, DateTime>();

    [HttpPost]
    [Route("generate")]
    [Authorize(Roles = Roles.UniversityAdmin)]
    public IActionResult GenerateToken()
    {
        var token = Guid.NewGuid().ToString();
        var expirationDate = DateTime.UtcNow.AddDays(7);

        Tokens[token] = expirationDate;

        return Ok(new { Token = token, ExpirationDate = expirationDate });
    }

    [HttpGet]
    [Route("validate/{token}")]
    public IActionResult ValidateToken(string token)
    {
        if (Tokens.TryGetValue(token, out var expirationDate) && expirationDate > DateTime.UtcNow)
        {
            return Ok(new { Valid = true });
        }

        return Ok(new { Valid = false });
    }
}
