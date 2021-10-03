using JWT_Authentication_NET_Core_Web_API_5._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Authentication_NET_Core_Web_API_5._0.Services
{
    public class UserService
    {
        private List<User> users = new List<User>()
        {
            new User{ Id=1, Username = "prjwl4_admin", Password = "12345", Role = "Admin", RefreshToken = "token1"},
            new User{ Id=2, Username = "prjwl4_user", Password = "12345", Role = "User", RefreshToken = "token2"}
        };

        public User AuthenticateUser(User userCredentials)
        {
            return users.Where(user => user.Username == userCredentials.Username && user.Password == userCredentials.Password).FirstOrDefault();
        }

        public User AuthenticateUser(string userName, string refreshToken)
        {
            return users.Where(user => user.Username == userName && user.RefreshToken == refreshToken).FirstOrDefault();
        }
    }
}
