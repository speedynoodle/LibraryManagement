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
                System.Console.WriteLine("2. Update an existing book");
                System.Console.WriteLine("3. Delete a book");
                System.Console.WriteLine("4. List all books");
                System.Console.WriteLine("5. View book details");
                System.Console.WriteLine("6. Exit");

                System.Console.Write("\nEnter your choice: ");
                if (int.TryParse(System.Console.ReadLine(), out int choice))
                {
                    System.Console.WriteLine();
                    
                    switch (choice)
                    {
                        case 1:
                            await AddBook();
                            break;
                        case 2:
                            await UpdateBook();
                            break;
                        case 3:
                            await DeleteBook();
                            break;
                        case 4:
                            await ListAllBooks();
                            break;
                        case 5:
                            await ViewBookDetails();
                            break;
                        case 6:
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
                
                System.Console.Write("ISBN (10 or 13 digits): ");
                book.ISBN = System.Console.ReadLine() ?? string.Empty;
                
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
        
        private async Task UpdateBook()
        {
            try
            {
                System.Console.WriteLine("--- UPDATE BOOK ---");
            
                System.Console.Write("Enter book ID: ");
                if (int.TryParse(System.Console.ReadLine(), out int id))
                {
                    var existingBook = await _bookService.GetBookById(id);
                    if (existingBook == null)
                    {
                        System.Console.WriteLine($"Book with ID {id} not found.");
                        return;
                    }

                    System.Console.WriteLine("Current book details:");
                    WriteBookDetails(existingBook);

                    System.Console.WriteLine("\nEnter new details (leave empty to keep current value):");
                    var title = ReadInputWithDefault("Title", existingBook.Title);
                    var author = ReadInputWithDefault("Author", existingBook.Author);
                    var isbn = ReadInputWithDefault("ISBN", existingBook.ISBN);

                    var updatedBook = new Book
                    {
                        Id = id,
                        Title = title,
                        Author = author,
                        ISBN = isbn
                    };
                    
                    if (await _bookService.UpdateBook(updatedBook))
                    {
                        System.Console.WriteLine("Book updated successfully!");
                    }
                    else
                    {
                        System.Console.WriteLine("Failed to update book. Please check your input and try again.");
                    }
                }
                else
                {
                    System.Console.WriteLine("Invalid book ID.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Oops! Something went wrong: {ex.Message}");
            }
        }
        
        private async Task ViewBookDetails()
        {
            System.Console.WriteLine("--- VIEW BOOK DETAILS ---");

            System.Console.Write("Enter book ID: ");
            if (int.TryParse(System.Console.ReadLine(), out int id))
            {
                var book = await _bookService.GetBookById(id);
                if (book == null)
                {
                    System.Console.WriteLine($"Book with ID {id} not found.");
                    return;
                }

                WriteBookDetails(book);
                
            }
            else
            {
                System.Console.WriteLine("Invalid book ID.");
            }
        }

        private async Task ListAllBooks()
        {
            System.Console.WriteLine("--- ALL BOOKS ---");
            
            var books = (await _bookService.GetAllBooks()).ToList();
            
            if (books.Count == 0)
            {
                System.Console.WriteLine("No books found in the library.");
                return;
            }

            System.Console.WriteLine($"Total books: {books.Count}\n");
            
            foreach (var book in books)
            {
                WriteBookDetails(book);
            }
        }

        private async Task DeleteBook()
        {
            System.Console.WriteLine("--- DELETE BOOK ---");
            System.Console.Write("Enter book ID: ");
            if (int.TryParse(System.Console.ReadLine(), out int id))
            {
                var existingBook = await _bookService.GetBookById(id);
                if (existingBook == null)
                {
                    System.Console.WriteLine($"Book with ID {id} not found.");
                    return;
                }
                
                System.Console.WriteLine("Book to delete:");
                WriteBookDetails(existingBook);

                System.Console.Write("\nAre you sure you want to delete this book? (y/n): ");
                var confirmation = System.Console.ReadLine()?.ToLower();
                
                if (confirmation == "y")
                {
                    if (await _bookService.DeleteBook(id))
                    {
                        System.Console.WriteLine("Book deleted successfully!");
                    }
                    else
                    {
                        System.Console.WriteLine("Failed to delete book.");
                    }
                }
                else
                {
                    System.Console.WriteLine("Book deletion cancelled.");
                }
            }
            else
            {
                System.Console.WriteLine("Invalid book ID.");
            }
        }
        
        private static string ReadInputWithDefault(string prompt, string defaultValue)
        {
            System.Console.Write($"{prompt} ({defaultValue}): ");
            var input = System.Console.ReadLine() ?? "";
            return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
        }

        private static void WriteBookDetails(Book book)
        {
            System.Console.WriteLine($"ID: {book.Id}");
            System.Console.WriteLine($"Title: {book.Title}");
            System.Console.WriteLine($"Author: {book.Author}");
            System.Console.WriteLine($"ISBN: {book.ISBN}");
        }
}