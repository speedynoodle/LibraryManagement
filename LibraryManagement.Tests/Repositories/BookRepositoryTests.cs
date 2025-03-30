using LibraryManagement.Core.Entities;
using LibraryManagement.Data.Context;
using LibraryManagement.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.Repositories;

public class BookRepositoryTests
{
    private readonly LibraryDbContext _dbContext;
    private readonly BookRepository _sut;

    public BookRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())  // Use a new DB name for each test
            .Options;

        _dbContext = new LibraryDbContext(dbContextOptions);
        _sut = new BookRepository(_dbContext);
    }

    [Fact]
    public async Task AddBook_ValidBook_AddsBookAndReturnsEntity()
    {
        // Arrange
        var book = new Book {Title = "Test Book", Author = "Test Author" };

        // Act
        var result = await _sut.Add(book);

        // Assert
        Assert.Equal(book.Id, result.Id);
        Assert.Contains(book, _dbContext.Books);
    }

    [Fact]
    public async Task AddBook_EmptyTitle_ThrowsValidationException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.Add(null));
    }
}