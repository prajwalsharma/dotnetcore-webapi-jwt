using JWT_Authentication_NET_Core_Web_API_5._0.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT_Authentication_NET_Core_Web_API_5._0.Services
{
    public class TokenService
    {

        private readonly double EXPIRE_HOURS = 0.5;
        private readonly string JWT_TOKEN = "5d2efc20-d921-4319-840c-053e8c6c120b";

        public string GenerateJwtToken(User user)
        {

            // 1. Convert secret key to byte array
            var key = Encoding.ASCII.GetBytes(JWT_TOKEN);

            // 2. Create a token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 3. Set properties for JWT token in a descriptor
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role),
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(EXPIRE_HOURS),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // 4. Create a token using token handler
            var token = tokenHandler.CreateToken(descriptor);

            // 5. Return the token string from token object
            return tokenHandler.WriteToken(token);

        }
    }
}
