using PRG281_Project_Milestone2.LibraryOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG281_Project_Milestone2
{
    public class Patron : User
    {
        public override string Role => "Patron";
        public List<Book> BorrowedBooks { get; } = new List<Book>();
        public int MaxLimit { get; set; } = 5;
        /*--------------------Borrow Book Process--------------------*/
        
        public Transaction BorrowBook(Book book)
        {
            if (book == null)
            {
                Console.WriteLine("The requested book was not found.");
                return null;
            }

            if (BorrowedBooks.Count >= MaxLimit)
            {
                Console.WriteLine("You have reached your maximum borrowing limit.");
                return null;
            }

            if (book.AvailableCopies <= 0)
            {
                Console.WriteLine("No available copies to borrow.");
                return null;
            }

            book.CheckOut();
            BorrowedBooks.Add(book);
            Console.WriteLine($"Book '{book.Title}' checked out successfully.");

            return new Transaction
            {
                BookISBN = book.ISBN,
                BookTitle = book.Title,
                PatronName = "Current Patron",
                TransactionType = TransactionType.CheckOut,
                TransactionDate = DateTime.Now
            };
        }

        /*--------------------Return Book Process--------------------*/
        public Transaction ReturnBook(Book book)
        {
            if (!BorrowedBooks.Contains(book))
            {
                Console.WriteLine("You have not borrowed this book.");
                return null;
            }

            book.Return();
            BorrowedBooks.Remove(book);
            Console.WriteLine($"Book '{book.Title}' returned successfully.");

            return new Transaction
            {
                BookISBN = book.ISBN,
                BookTitle = book.Title,
                PatronName = "Current Patron",
                TransactionType = TransactionType.Return,
                TransactionDate = DateTime.Now
            };
        }
    }
}
