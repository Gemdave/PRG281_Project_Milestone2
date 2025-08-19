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
            Console.WriteLine("--- Library Management System ---");


            LibrarySystem library = new LibrarySystem();
            Staff librarian = new Staff();
            Patron gerald = new Patron();


            library.OverdueAlert += librarian.HandleOverdueAlert;


            library.Books.Add(new Book { ISBN = "978-0131103627", Title = "C# Programming", Author = "John Doe" });
            library.Books.Add(new Book { ISBN = "978-0321765723", Title = "Data Structures", Author = "Jane Smith" });
            Console.WriteLine("\n--- Demonstrating Error Checking ---");
            try
            {
                gerald.BorrowBook(null);
            }
            catch (BookNotFoundException ex)
            {
                Console.WriteLine($"Caught an error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Book book1 = library.Books[0];
            Book book2 = library.Books[1];
            Book book3 = new Book { Title = "A Third Book" };
            library.Books.Add(book3);

            gerald.BorrowBook(book1);
            gerald.BorrowBook(book2);
            gerald.BorrowBook(book3);

            try
            {
                Book extraBook = new Book { Title = "An Extra Book" };
                gerald.BorrowBook(extraBook);
            }
            catch (MaxBorrowLimitException ex)
            {
                Console.WriteLine($"Caught an error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.WriteLine("\n--- Demonstrating Notifications ---");
            library.MonitorOverdue();

            Console.WriteLine("\nProgram finished.");
            Console.WriteLine("\nPress any key to exit");
            Console.ReadLine();
        }
        private static Library library = new Library();
        private static Patron currentUser = new Patron();

        static string GetInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
        /*---------------Added Book operations strait from Books---------------*/
        static void BookOperations()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nBOOK OPERATIONS");
                Console.WriteLine("1. Check Out");
                Console.WriteLine("2. Return");
                Console.WriteLine("3. Add New Book");
                Console.WriteLine("4. Back to Main Menu");

                string choice = GetInput("Select action: ");

                switch (choice)
                {
                    case "1": // Check Out
                        string isbn = GetInput("Enter ISBN to check out: ");
                        Book bookToCheckout = library.FindBook(isbn);

                        if (bookToCheckout == null)
                        {
                            Console.WriteLine("Book not found in library");
                        }
                        else if (currentUser.BorrowedBooks.Count >= 5)
                        {
                            Console.WriteLine("You have reached the 5-book limit.");
                        }
                        else if (bookToCheckout.AvailableCopies <= 0)
                        {
                            Console.WriteLine("No copies available");
                        }
                        else
                        {
                            currentUser.BorrowBook(bookToCheckout);
                            Console.WriteLine("Successfully checked out the book: " + bookToCheckout.Title);
                        }
                        break;

                    case "2": // Return book
                        string returnIsbn = GetInput("Enter ISBN to return: ");
                        Book bookToReturn = currentUser.BorrowedBooks.FirstOrDefault(b => b.ISBN == returnIsbn);

                        if (bookToReturn == null)
                        {
                            Console.WriteLine("You have not borrowed this book");
                        }
                        else if (bookToReturn.AvailableCopies >= bookToReturn.TotalCopies)
                        {
                            Console.WriteLine("Cannot return more copies than original stock");
                        }
                        else
                        {
                            currentUser.ReturnBook(bookToReturn);
                            Console.WriteLine("Book successfully returned: " + bookToReturn.Title);
                        }
                        break;

                    case "3": // Add New Book
                        Console.WriteLine("\nADD NEW BOOK");
                        string newIsbn = GetInput("Enter ISBN: ");

                        if (library.FindBook(newIsbn) != null)
                        {
                            Console.WriteLine("ISBN already exists");
                        }
                        else if (!int.TryParse(GetInput("Enter total copies: "), out int totalCopies) ||
                                 !int.TryParse(GetInput("Enter available copies: "), out int availableCopies))
                        {
                            Console.WriteLine("Copies must be numbers");
                        }
                        else if (availableCopies > totalCopies)
                        {
                            Console.WriteLine("Available copies cannot exceed total copies");
                        }
                        else
                        {
                            library.AddBook(new Book
                            {
                                ISBN = newIsbn,
                                Title = GetInput("Enter Title: "),
                                Author = GetInput("Enter Author: "),
                                TotalCopies = totalCopies,
                                AvailableCopies = availableCopies
                            });
                            Console.WriteLine("Book added successfully!");
                        }
                        break;

                    case "4":
                        return; // Exit to main menu

                    default:
                        Console.WriteLine("Invalid choice, please try again");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
    public class Transaction { }

}

