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
    
    [Fact]
    public async Task GetById_ValidId_ReturnsBook()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Test Book", Author = "Test Author" };
        await _dbContext.Books.AddAsync(book);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(book.Id, result.Id);
        Assert.Equal(book.Title, result?.Title);
        Assert.Equal(book.Author, result?.Author);
    }
    
    [Fact]
    public async Task GetAll_ReturnsAll()
    {
        // Arrange
        var book1 = new Book { Id = 1, Title = "Test Book", Author = "Test Author" };
        var book2 = new Book { Id = 2, Title = "Test Book2", Author = "Test Author2" };
        var book3 = new Book { Id = 3, Title = "Test Book3", Author = "Test Author3" };
        
        await _dbContext.Books.AddRangeAsync(book1, book2, book3);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Contains(result, b => b.Id == 1 && b.Title == "Test Book" && b.Author == "Test Author");
        Assert.Contains(result, b => b.Id == 2 && b.Title == "Test Book2" && b.Author == "Test Author2");
        Assert.Contains(result, b => b.Id == 3 && b.Title == "Test Book3" && b.Author == "Test Author3");
    }
    
    [Fact]
    public async Task Update_ShouldUpdateExistingBook()
    {
        // Arrange
        var book = new Book
        {
            Title = "Original Title",
            Author = "Original Author",
        };
        
        await _sut.Add(book);

        // Act
        book.Title = "Updated Title";
        book.Author = "Updated Author";
        await _sut.Update(book);
        var result =  await _sut.GetById(book.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("Updated Author", result.Author);
    }
    
    [Fact]
    public async Task Remove_ShouldRemoveBookFromRepository()
    {
        // Arrange
        var book = new Book
        {
            Title = "Test Book",
            Author = "Test Author",

        };
        await _sut.Add(book);

        // Act
        await _sut.Remove(book);
        var result = await _sut.GetById(book.Id);

        // Assert
        Assert.Null(result);
    }
}