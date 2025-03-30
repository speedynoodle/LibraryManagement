using LibraryManagement.Core.Entities;
using LibraryManagement.Services.Validators;

namespace Tests.Validators;

public class BookValidatorTests
{
    private readonly BookValidator _sut;

    public BookValidatorTests()
    {
        _sut = new BookValidator();
    }

    [Fact]
    public void IsValid_ValidBook_ReturnsTrueAndEmptyMessage()
    {
        // Arrange
        var book = new Book { Title = "Test Book", Author = "Test Author" };

        // Act
        var result = _sut.IsValid(book);

        // Assert
        Assert.True(result.isValid);
        Assert.Empty(result.errorMessage);
    }

    [Fact]
    public void IsValid_EmptyTitle_ReturnsFalseAndTitleError()
    {
        // Arrange
        var book = new Book { Title = "", Author = "Test Author" };

        // Act
        var result = _sut.IsValid(book);

        // Assert
        Assert.False(result.isValid);
        Assert.Equal("Title is required", result.errorMessage);
    }

    [Fact]
    public void IsValid_EmptyAuthor_ReturnsFalseAndAuthorError()
    {
        // Arrange
        var book = new Book { Title = "Test Book", Author = "" };

        // Act
        var result = _sut.IsValid(book);

        // Assert
        Assert.False(result.isValid);
        Assert.Equal("Author is required", result.errorMessage);
    }

    [Fact]
    public void IsValid_EmptyTitleAndAuthor_ReturnsFalseAndBothErrors()
    {
        // Arrange
        var book = new Book { Title = "", Author = "" };

        // Act
        var result = _sut.IsValid(book);

        // Assert
        Assert.False(result.isValid);
        Assert.Equal("Title is required, Author is required", result.errorMessage);
    }
}