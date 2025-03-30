using System.ComponentModel.DataAnnotations;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Console;

public class LibraryConsoleUI
{
    private readonly IBookService _bookService;

    public LibraryConsoleUI(IBookService bookService)
    {
        _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
    }
        public async Task Run()
        {
            var isRunning = true;
            
            while (isRunning)
            {
                System.Console.Clear();
                System.Console.WriteLine("LIBRARY MANAGEMENT SYSTEM");
                System.Console.WriteLine("1. Add a book");

                System.Console.Write("\nEnter your choice: ");
                if (int.TryParse(System.Console.ReadLine(), out int choice))
                {
                    System.Console.WriteLine();
                    
                    switch (choice)
                    {
                        case 1:
                            await AddBook();
                            break;
                        case 7:
                            isRunning = false;
                            break;
                        default:
                            System.Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                else
                {
                    System.Console.WriteLine("Invalid input. Please enter a number.");
                }

                if (isRunning)
                {
                    System.Console.WriteLine("\nPress any key to continue...");
                    System.Console.ReadKey();
                }
            }
        }
     
        private async Task AddBook()
        {
            try
            {
                System.Console.WriteLine("--- ADD NEW BOOK ---");
            
                var book = new Book();

                System.Console.Write("Title: ");
                book.Title = System.Console.ReadLine() ?? string.Empty;

                System.Console.Write("Author: ");
                book.Author = System.Console.ReadLine() ?? string.Empty;
                
                var bookId =  await _bookService.AddBook(book);
                System.Console.WriteLine(bookId > 0
                    ? $"Book added successfully with ID: {bookId}!"
                    : "Failed to add book. Please check your input and try again.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Oops! Something went wrong: {ex.Message}");
            }
        }
}