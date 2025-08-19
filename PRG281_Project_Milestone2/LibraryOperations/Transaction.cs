using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG281_Project_Milestone2.LibraryOperations
{
    public enum TransactionType
    {
        CheckOut,
        Return
    }

    public class Transaction
    {
        public string BookISBN { get; set; }
        public string BookTitle { get; set; }
        public string PatronName { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
    }

}
