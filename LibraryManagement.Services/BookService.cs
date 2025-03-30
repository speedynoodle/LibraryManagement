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

    /// <summary>
    /// Gets a book by its ID
    /// </summary>
    /// <param name="id">The book ID</param>
    /// <returns>The book if found, otherwise null</returns>
    public async Task<Book?> GetBookById(int id)
    {
        return await _bookRepository.GetById(id);
    }

    public async Task<IEnumerable<Book>> GetAllBooks()
    {
       return await _bookRepository.GetAll();
    }

    public async Task<bool> UpdateBook(Book updatedBook)
    {
        ArgumentNullException.ThrowIfNull(updatedBook);

        var (isValid, errorMessage) = _bookValidator.IsValid(updatedBook);
        if (!isValid)
        {
            throw new ValidationException(errorMessage); 
        }

        await _bookRepository.Update(updatedBook);
        return true;
    }

    public async Task<bool> DeleteBook(int id)
    {
        var book = await _bookRepository.GetById(id);
        if (book == null)
        {
            return false; 
        }
        
        await _bookRepository.Remove(book);
        return true;
    }
}