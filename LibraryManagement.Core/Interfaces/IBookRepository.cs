using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces;

public interface IBookRepository  : IRepository<Book>
{
    Task<bool> IsbnExists(string isbn, int? excludeBookId = null);
}