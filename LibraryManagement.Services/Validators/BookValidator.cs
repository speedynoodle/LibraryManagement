using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Services.Validators;

public class BookValidator : IValidator<Book>
{
    private readonly IValidator<string> _isbnValidator;

    public BookValidator(IValidator<string> isbnValidator)
    {
        _isbnValidator = isbnValidator ?? throw new ArgumentNullException(nameof(isbnValidator));
    }
    public (bool isValid, string? errorMessage) IsValid(Book book)
    {
        var errorMsgs = new List<string>();
        
        if (string.IsNullOrWhiteSpace(book.Title))
        {
            errorMsgs.Add("Title is required");
        }
        
        if (string.IsNullOrWhiteSpace(book.Author))
        {
            errorMsgs.Add("Author is required");
        } 
        
        var (isValidIsbn, isbnErrorMsg) = _isbnValidator.IsValid(book.ISBN);

        if (!isValidIsbn)
        {
            errorMsgs.Add(isbnErrorMsg);
        }
        
        return errorMsgs.Any()
            ? (false, string.Join(", ", errorMsgs)) 
            : (true, string.Empty);
    }
}