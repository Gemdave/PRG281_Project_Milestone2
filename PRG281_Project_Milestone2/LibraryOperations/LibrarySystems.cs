using PRG281_Project_Milestone2.LibraryOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG281_Project_Milestone2
{
    public delegate void OverdueAlertHandler(object sender, OverdueEventArgs e);
    public class LibrarySystem
    {
        public event OverdueAlertHandler OverdueAlert;

        public List<Book> Books { get; } = new List<Book>();
        public List<Patron> Patrons { get; } = new List<Patron>();
        public List<Transaction> Transactions { get; } = new List<Transaction>();

        public void MonitorOverdue()
        {
            // Leave as is per your request
            OnOverdueAlert(new OverdueEventArgs("Lesedi Mawewe", "C# Programming"));
        }

        protected virtual void OnOverdueAlert(OverdueEventArgs e)
        {
            OverdueAlert?.Invoke(this, e);
        }
    }
}