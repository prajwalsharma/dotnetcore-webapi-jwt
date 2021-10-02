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
            new User{ Id=1, Username = "prjwl4_admin", Password = "12345", Role = "Admin"},
            new User{ Id=2, Username = "prjwl4_user", Password = "12345", Role = "User"}
        };

        public User AuthenticateUser(User userCredentials)
        {
            return users.Where(user => user.Username == userCredentials.Username && user.Password == userCredentials.Password).FirstOrDefault();
        }
    }
}
