using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces;

public interface IBookService
{
    Task<int> AddBook(Book book);
    Task<Book?> GetBookById(int id);
    Task<IEnumerable<Book>> GetAllBooks();
    Task<bool> UpdateBook(Book updatedBook);
    Task<bool> DeleteBook(int id);
}