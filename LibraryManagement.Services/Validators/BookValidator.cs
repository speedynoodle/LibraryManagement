using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Services.Validators;

public class BookValidator : IValidator<Book>
{
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
        
        return errorMsgs.Any()
            ? (false, string.Join(", ", errorMsgs)) 
            : (true, string.Empty);
    }
}