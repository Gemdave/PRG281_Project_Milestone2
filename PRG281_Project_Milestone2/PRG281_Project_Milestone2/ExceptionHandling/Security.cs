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
        // Hash a password using SHA256
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        // Verify entered password against stored hash
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredHash = HashPassword(enteredPassword);
            return enteredHash.Equals(storedHash);
        }

        // Simple login system for demonstration
        public static bool Login(string username, string password)
        {
            // In a real system, this would be stored in a DB/file
            string storedUser = "admin";
            string storedHash = HashPassword("password123");

            if (username == storedUser && VerifyPassword(password, storedHash))
            {
                Console.WriteLine("✅ Login successful!");
                return true;
            }
            Console.WriteLine("❌ Invalid login.");
            return false;
        }
    }
}
