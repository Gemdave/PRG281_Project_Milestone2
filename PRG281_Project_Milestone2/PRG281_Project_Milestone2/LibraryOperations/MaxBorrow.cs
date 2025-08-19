using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG281_Project_Milestone2
{
    public class MaxBorrowLimitException : Exception
    {
        public MaxBorrowLimitException(string message) : base(message)
        {
        }
    }

}
