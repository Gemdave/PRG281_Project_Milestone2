using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG281_Project_Milestone2
{
    public class OverdueEventArgs : EventArgs
    {
        public string PatronName { get; set; }
        public string BookTitle { get; set; }
        public OverdueEventArgs(string patronName, string bookTitle)
        {
            PatronName = patronName;
            BookTitle = bookTitle;
        }
    }
}
