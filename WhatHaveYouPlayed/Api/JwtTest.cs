using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WhatHaveYouPlayed.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JwtTest : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUserInfo()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(new
            {
                message = "Dostęp Autoryzowany!",
                user = userName
            });
        }
    }
}
