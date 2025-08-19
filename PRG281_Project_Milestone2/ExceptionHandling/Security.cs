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
        // Verify entered password against stored hash
        public static bool VerifyPassword(string enteredPassword, string storedPass)
        {
            return enteredPassword.Equals(storedPass);
        }

        // Simple login system for demonstration
        public static bool Login(string username, string password)
        {
            // In a real system, this would be stored in a DB/file
            string storedUser = "admin";
            string storedPass = "password123";

            if (username == storedUser && VerifyPassword(password, storedPass))
            {
                Console.WriteLine("Login successful!");
                return true;
            }
            Console.WriteLine("Invalid login.");
            return false;
        }
    }
}
