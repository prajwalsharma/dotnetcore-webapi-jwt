using JWT_Authentication_NET_Core_Web_API_5._0.Models;
using JWT_Authentication_NET_Core_Web_API_5._0.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        // Public Login Route
        // Generate JWT and client can store the token in sessionStorage or localStorage
        [HttpPost("auth/login-jwt")]
        [AllowAnonymous]
        public IActionResult LoginJWT([FromBody] User userCredentials)
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

        // Public Login Route
        // Generate JWT & set it in response cookie, which client cannot store anywhere
        [HttpPost("auth/login-cookie")]
        [AllowAnonymous]
        public IActionResult LoginCookie([FromBody] User userCredentials)
        {

            dynamic response = Unauthorized();

            var user = userService.AuthenticateUser(userCredentials);

            if (user != null)
            {
                string jwtToken = tokenService.GenerateJwtToken(user);
                string newRefreshToken = "token1";// Guid.NewGuid().ToString();
                user.RefreshToken = newRefreshToken;

                Response.Cookies.Append("X-Access-Token", jwtToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true });
                Response.Cookies.Append("X-Username", user.Username, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true });
                Response.Cookies.Append("X-Refresh-Token", user.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true });

                response = Ok();
            }

            return response;

        }

        // Refresh & generate new JWT token and cookies
        [HttpGet("auth/refresh")]
        [AllowAnonymous]
        public IActionResult Refresh()
        {
            string userName = Request.Cookies["X-Username"];
            string refreshToken = Request.Cookies["X-Refresh-Token"];

            if (userName == null || refreshToken == null)
            {
                return BadRequest();
            }

            var user = userService.AuthenticateUser(userName, refreshToken);

            if (user == null) {
                return BadRequest();
            }

            user.RefreshToken = "token3";
            string jwtToken = tokenService.GenerateJwtToken(user);

            // Update database with new refresh token...

            Response.Cookies.Append("X-Access-Token", jwtToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true });
            Response.Cookies.Append("X-Username", user.Username, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true });
            Response.Cookies.Append("X-Refresh-Token", user.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true });

            return Ok();

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
