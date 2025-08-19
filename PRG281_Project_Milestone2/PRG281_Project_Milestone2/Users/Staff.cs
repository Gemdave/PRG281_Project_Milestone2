using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG281_Project_Milestone2
{
    public class Staff : User
    {
        public string[] Permissions { get; set; }

        public void HandleOverdueAlert(object sender, OverdueEventArgs e)
        {
            Console.WriteLine($"\n--- OVERDUE BOOK ALERT ---");
            Console.WriteLine($"Patron: {e.PatronName}");
            Console.WriteLine($"Book: '{e.BookTitle}'");
            Console.WriteLine($"Action Required: Notify patron to return the book.");
            Console.WriteLine($"--------------------------\n");
        }
    }

}
