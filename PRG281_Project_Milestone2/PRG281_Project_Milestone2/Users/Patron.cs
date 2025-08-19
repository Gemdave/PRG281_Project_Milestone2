using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG281_Project_Milestone2
{
    public class Patron : User
    {
        public List<Book> BorrowedBooks { get; } = new List<Book>();
        public int MaxLimit { get; set; } = 3; // Setting a sample limit

        public void BorrowBook(Book book)
        {
            if (book == null)
            {
                throw new BookNotFoundException("The requested book was not found.");
            }

            if (BorrowedBooks.Count >= MaxLimit)
            {
                throw new MaxBorrowLimitException("You have reached your maximum borrowing limit.");
            }

            // Additional logic to check availability and check out the book
            BorrowedBooks.Add(book);
            book.CheckOut();
            Console.WriteLine($"Book '{book.Title}' checked out successfully.");
        }
    }

}
