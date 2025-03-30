using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces;

public interface IBookService
{
    Task<int> AddBook(Book book);
}