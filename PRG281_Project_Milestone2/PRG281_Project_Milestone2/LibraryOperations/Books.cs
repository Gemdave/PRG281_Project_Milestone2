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
        public int AvailableCopies { get; set; }  // Tracks the current stock available 
        public int TotalCopies { get; set; }     // Original inventory amount

        //CHECK OUT PROCESS
        public void CheckOut()
        {
            if (AvailableCopies > 0)
                AvailableCopies--;  // Reduce available count
            else
                Console.WriteLine();
        }

        //RETURN PROCESS
        public void Return()
        {
            if (AvailableCopies < TotalCopies)
                AvailableCopies++;  // Restoring the stock
            else
                Console.WriteLine();
        }
        //CHECKOUT INTERGRATION



    }
    public class Patron
    {
        public List<Book> BorrowedBooks { get; } = new List<Book>();

        // BORROW BOOK PROCESS
        public void BorrowBook(Book book)
        {
            if (BorrowedBooks.Count >= 5)
                Console.WriteLine();

            book.CheckOut();
            // Calls the Book's CheckOut()
            BorrowedBooks.Add(book); // Track patron's borrowed books
        }

        // RETURN BOOK PROCESS
        public void ReturnBook(Book book)
        {
            book.Return();
            // Calls Book's Return()
            BorrowedBooks.Remove(book);
        }

    }
    public class Library
    {
        private List<Book> books = new List<Book>();

        // ADD NEW BOOK PROCESS
        public void AddBook(Book newBook)
        {
            lock (books)  // Thread-safe operation
            {
                if (books.Exists(b => b.ISBN == newBook.ISBN))
                    Console.WriteLine();
                else
                    books.Add(newBook);
            }
        }

        // FIND BOOK PROCESS
        public Book FindBook(string isbn)
        {
            var book = books.Find(b => b.ISBN == isbn);
            if (book == null)
                Console.WriteLine();
            return book;
        }
    }
}
