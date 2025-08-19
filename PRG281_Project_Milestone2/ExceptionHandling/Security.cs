using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PRG281_Project_Milestone2
{
    public static class Security
    {
        public static List<User> Users = new List<User>
        {
            new Staff { Username = "admin", Password = "password123" },
            new Patron { Username = "gerald", Password = "booklover" }
        };
        public static bool VerifyPassword(string enteredPassword, string storedPass)
        {
            return enteredPassword.Equals(storedPass);
        }

        public static User Login(string username, string password)
        {
            return Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
