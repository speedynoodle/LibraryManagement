using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data.Repositories;

public class BookRepository  : IBookRepository
{
    private readonly LibraryDbContext _context;
    
    public BookRepository(LibraryDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<Book?> Add(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book?> GetById(int id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task<IEnumerable<Book>> GetAll()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task Update(Book entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var existingEntity = await _context.Books.FindAsync(entity.Id);
        if (existingEntity != null)
        {
            _context.Entry(existingEntity).State = EntityState.Detached;
        }

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Remove(Book entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Books.Remove(entity);
        await _context.SaveChangesAsync();
    }
}