using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG281_Project_Milestone2
{

    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int AvailableCopies { get; set; }  // Current stock available 
        public int TotalCopies { get; set; }     // Original inventory amount

        // CHECK OUT PROCESS
        public void CheckOut()
        {
            if (AvailableCopies > 0)
                AvailableCopies--;
            else
                Console.WriteLine("No copies available to check out.");
        }

        // RETURN PROCESS
        public void Return()
        {
            if (AvailableCopies < TotalCopies)
                AvailableCopies++;
            else
                Console.WriteLine("All copies already returned.");
        }
    }
}