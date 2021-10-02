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

        // Protected Route
        [HttpGet("api/summaries")]
        [Authorize]
        public IActionResult GetSummaries()
        {
            return Ok(Summaries);
        }

        // Public Route
        [HttpGet("/landing-page")]
        [AllowAnonymous]
        public IActionResult LandingPage()
        {
            return Ok("This is a public page!");
        }

        // Public Route
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

        // Protected Route - For Admins
        [HttpGet("api/admin-dashboard")]
        [Authorize(Roles = "Admin")]
        public string AdminDashboard()
        {
            return "This is a protected route! Only admins can access it.";
        }

        // Protected Route - For Users
        [HttpGet("api/user-dashboard")]
        [Authorize(Roles = "User")]
        public string UserDashboard()
        {
            return "This is a protected route! Only users can access it.";
        }
    }
}
