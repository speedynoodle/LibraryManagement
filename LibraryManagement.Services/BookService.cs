using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IValidator<Book> _bookValidator;

    public BookService(IBookRepository bookRepository,  IValidator<Book> bookValidator)
    {
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        _bookValidator = bookValidator ?? throw new ArgumentNullException(nameof(bookValidator));
    }
    
    /// <summary>
    /// Adds a new book to the system
    /// </summary>
    /// <param name="book">The book to add</param>
    /// <returns>The ID of the newly added book if successful, otherwise -1</returns>
    public async Task<int> AddBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        var (isValid, errorMessage) = _bookValidator.IsValid(book);
        if (!isValid)
        {
            throw new ValidationException(errorMessage); 
        }

        await _bookRepository.Add(book);
        return book.Id;
    }
}