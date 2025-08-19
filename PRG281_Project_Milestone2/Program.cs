using PRG281_Project_Milestone2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PRG281_Project_Milestone2
{
    internal class Program
    {
        private static Library library = new Library();
        private static Patron currentUser = new Patron();
        private static LibrarySystem librarySystem = new LibrarySystem();
        public string UserRole;

        static void Main(string[] args)
        {
            LibrarySystem librarySystem = new LibrarySystem();
            User loggedInUser = null;
            bool Exit = false;
            while (!Exit)
            {
                Console.Clear();
                Console.WriteLine("Login?:");
                Console.WriteLine("1. Continue To login");
                Console.WriteLine("2. Exit");
                string choice = GetInput("Select an option: ");
                switch (choice)
                {
                    case "1":
                        while (loggedInUser == null)
                        {
                            Console.Clear();
                            Console.WriteLine("Please login to access the Library System.");
                            Console.Write("Username: ");
                            string username = Console.ReadLine();
                            Console.Write("Password: ");
                            string password = Console.ReadLine();
                            loggedInUser = Security.Login(username, password);
                            if (loggedInUser == null)
                            {
                                Console.WriteLine("Invalid credentials. Try again.");
                            }
                            else
                            {
                                Console.WriteLine("Login Succes");
                            }
                            Console.ReadLine();
                        }

                        Console.WriteLine($"Welcome, {loggedInUser.Username} ({loggedInUser.Role})");

                        // Subscribe librarian to overdue alerts
                        Staff librarian = new Staff();
                        librarySystem.OverdueAlert += librarian.HandleOverdueAlert;

                        Console.WriteLine("--- Library Management System ---");

                        InitializeLibrary();

                        ShowMainMenu(loggedInUser);
                        loggedInUser = null;
                        break;
                    case "2":
                        Exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid selection");
                        return;
                }                
            }
            Console.Clear() ;
            Console.WriteLine("Program finished. Press any key to exit.");
            Console.ReadKey();
        }
        /*--------------------Create library--------------------*/
        static void InitializeLibrary()
        {
            library.AddBook(new Book { ISBN = "978-0131103627", Title = "C# Programming", Author = "John Doe", TotalCopies = 3, AvailableCopies = 3 });
            library.AddBook(new Book { ISBN = "978-0321765723", Title = "Data Structures", Author = "Jane Smith", TotalCopies = 2, AvailableCopies = 2 });
            // Sync with LibrarySystem
            librarySystem.Books.AddRange(library.Books);
        }
        /*--------------------Main Menu--------------------*/
        static void ShowMainMenu(User userRole)
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
                        if (userRole is Staff staff)
                        {
                            ShowStaffMenu(librarySystem);
                        }
                        else if (userRole is Patron patron)
                        {
                            ShowPatronMenu(patron, librarySystem);
                        }
                        break;
                    case "2":
                        librarySystem.MonitorOverdue();
                        Console.WriteLine("Press any key to continue...");
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
        /*--------------------Staff Menu--------------------*/
        static void ShowStaffMenu(LibrarySystem library)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== STAFF MENU ===");
                Console.WriteLine("1. Monitor Overdue Books");
                Console.WriteLine("2. Add New Book");
                Console.WriteLine("3. Exit");

                string choice = GetInput("Enter choice: ");
                switch (choice)
                {
                    case "1":
                        library.MonitorOverdue();
                        break;
                    case "2":
                        AddNewBook();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
        /*--------------------Patron Menu--------------------*/
        static void ShowPatronMenu(Patron patron, LibrarySystem library)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== PATRON MENU ===");
                Console.WriteLine("1. Borrow Book");
                Console.WriteLine("2. Return Book");
                Console.WriteLine("3. Exit");

                string choice = GetInput("Enter choice: ");
                switch (choice)
                {
                    case "1":
                        CheckOutBook();
                        break;
                    case "2":
                        ReturnBook();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
        /*--------------------Borrowed Books function--------------------*/
        static void ShowBorrowedBooks()
        {
            Console.WriteLine("Your Borrowed Books:");
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
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        /*--------------------Checkout Function--------------------*/
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
        /*--------------------Return Function--------------------*/
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
        /*--------------------New Book Function--------------------*/
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
            //Ensure that the user enters Integer values
            int totalCopies = 1, availableCopies = 1;
            try
            {
                totalCopies = int.Parse(GetInput("Enter total copies: "));
                availableCopies = int.Parse(GetInput("Enter available copies: "));
            }
            catch
            {
                Console.WriteLine("Copies must be numbers.");
                return ;
            }
            //Ensure there are not more AvailableCopie than TotalCopies
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
        /*--------------------User Input Shortcut--------------------*/
        static string GetInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}