using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG281_Project_Milestone2
{
    internal class Program
    {
        private static Library library = new Library();
        private static Patron currentUser = new Patron();
        private static LibrarySystem librarySystem = new LibrarySystem();

        static void Main(string[] args)
        {
            Console.WriteLine("Please login to access the Library System.");
            Console.Write("Username: ");
            string user = Console.ReadLine();
            Console.Write("Password: ");
            string pass = Console.ReadLine();

            if (!Security.Login(user, pass))
            {
                Console.WriteLine("Exiting program...");
                return;
            }

            // Subscribe librarian to overdue alerts
            Staff librarian = new Staff();
            librarySystem.OverdueAlert += librarian.HandleOverdueAlert;

            Console.WriteLine("\n--- Library Management System ---");

            InitializeLibrary();

            ShowMainMenu();

            Console.WriteLine("\nProgram finished. Press any key to exit.");
            Console.ReadKey();
        }

        static void InitializeLibrary()
        {
            library.AddBook(new Book { ISBN = "978-0131103627", Title = "C# Programming", Author = "John Doe", TotalCopies = 3, AvailableCopies = 3 });
            library.AddBook(new Book { ISBN = "978-0321765723", Title = "Data Structures", Author = "Jane Smith", TotalCopies = 2, AvailableCopies = 2 });

            // Sync with LibrarySystem
            librarySystem.Books.AddRange(library.Books);
        }

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Book Operations");
                Console.WriteLine("2. Monitor Overdue Books");
                Console.WriteLine("3. View Borrowed Books");
                Console.WriteLine("4. Exit");

                string choice = GetInput("Select an option: ");

                switch (choice)
                {
                    case "1":
                        BookOperations();
                        break;
                    case "2":
                        librarySystem.MonitorOverdue();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                    case "3":
                        ShowBorrowedBooks();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
        }

        static void ShowBorrowedBooks()
        {
            Console.WriteLine("\nYour Borrowed Books:");
            if (!currentUser.BorrowedBooks.Any())
            {
                Console.WriteLine("You have no borrowed books.");
            }
            else
            {
                foreach (var book in currentUser.BorrowedBooks)
                {
                    Console.WriteLine($"- {book.Title} (ISBN: {book.ISBN})");
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void BookOperations()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Book Operations:");
                Console.WriteLine("1. Check Out Book");
                Console.WriteLine("2. Return Book");
                Console.WriteLine("3. Add New Book");
                Console.WriteLine("4. Back to Main Menu");

                string choice = GetInput("Select an action: ");

                switch (choice)
                {
                    case "1":
                        CheckOutBook();
                        break;
                    case "2":
                        ReturnBook();
                        break;
                    case "3":
                        AddNewBook();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void CheckOutBook()
        {
            string isbn = GetInput("Enter ISBN to check out: ");
            Book book = library.FindBook(isbn);

            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            // Exception handling can be added/commented here if desired

            var transaction = currentUser.BorrowBook(book);
            if (transaction != null)
            {
                librarySystem.Transactions.Add(transaction);
            }
        }

        static void ReturnBook()
        {
            string isbn = GetInput("Enter ISBN to return: ");
            Book book = currentUser.BorrowedBooks.FirstOrDefault(b => b.ISBN == isbn);

            if (book == null)
            {
                Console.WriteLine("You have not borrowed this book.");
                return;
            }

            var transaction = currentUser.ReturnBook(book);
            if (transaction != null)
            {
                librarySystem.Transactions.Add(transaction);
            }
        }

        static void AddNewBook()
        {
            string isbn = GetInput("Enter ISBN: ");

            if (library.FindBook(isbn) != null)
            {
                Console.WriteLine("ISBN already exists.");
                return;
            }

            string title = GetInput("Enter Title: ");
            string author = GetInput("Enter Author: ");
            if (!int.TryParse(GetInput("Enter total copies: "), out int totalCopies) ||
                !int.TryParse(GetInput("Enter available copies: "), out int availableCopies))
            {
                Console.WriteLine("Copies must be numbers.");
                return;
            }

            if (availableCopies > totalCopies)
            {
                Console.WriteLine("Available copies cannot exceed total copies.");
                return;
            }

            Book newBook = new Book
            {
                ISBN = isbn,
                Title = title,
                Author = author,
                TotalCopies = totalCopies,
                AvailableCopies = availableCopies
            };

            library.AddBook(newBook);
            librarySystem.Books.Add(newBook);
            Console.WriteLine("Book added successfully.");
        }

        static string GetInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}