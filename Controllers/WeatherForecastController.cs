using JWT_Authentication_NET_Core_Web_API_5._0.Models;
using JWT_Authentication_NET_Core_Web_API_5._0.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Authentication_NET_Core_Web_API_5._0.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly TokenService tokenService;
        private readonly UserService userService;

        public WeatherForecastController()
        {
            tokenService = new TokenService();
            userService = new UserService();
        }

        [HttpGet("api/summaries")]
        [Authorize]
        public IActionResult GetSummaries()
        {
            return Ok(Summaries);
        }

        [HttpGet("/landing-page")]
        [AllowAnonymous]
        public IActionResult LandingPage()
        {
            return Ok("This is a public page!");
        }

        [HttpPost("api/login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] User userCredentials)
        {

            dynamic response = Unauthorized();

            var user = userService.AuthenticateUser(userCredentials);

            if (user != null)
            {
                var jwtToken = tokenService.GenerateJwtToken(user);
                userCredentials.Password = "";
                var responseObject = new { user, token = jwtToken };
                response = Ok(responseObject);
            }

            return response;

        }
    }
}
