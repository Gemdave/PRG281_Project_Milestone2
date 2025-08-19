using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG281_Project_Milestone2
{
    public class Library
    {
        private List<Book> books = new List<Book>();

        public List<Book> Books => books;

        // ADD NEW BOOK PROCESS
        public void AddBook(Book newBook)
        {
            lock (books)  // Thread-safe operation
            {
                if (books.Exists(b => b.ISBN == newBook.ISBN))
                    Console.WriteLine("Book with this ISBN already exists.");
                else
                    books.Add(newBook);
            }
        }

        // FIND BOOK PROCESS
        public Book FindBook(string isbn)
        {
            var book = books.Find(b => b.ISBN == isbn);
            if (book == null)
                Console.WriteLine("Book not found.");
            return book;
        }
    }
}
