using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Data.Context;

namespace LibraryManagement.Data.Repositories;

public class BookRepository  : IBookRepository
{
    private readonly LibraryDbContext _context;
    
    public BookRepository(LibraryDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<Book?> Add(Book entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _context.Books.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}