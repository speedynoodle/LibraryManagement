using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Services.Validators;
using Moq;

namespace Tests.Validators;

public class BookValidatorTests
{
    private readonly BookValidator _sut;
    private readonly Mock<IValidator<string>> _mockIsbnValidator;

    public BookValidatorTests()
    {
        _mockIsbnValidator = new Mock<IValidator<string>>();
        _mockIsbnValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(( true, null)); 
        
        _sut = new BookValidator(_mockIsbnValidator.Object);
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
        _mockIsbnValidator.Verify(v => v.IsValid(It.IsAny<string>()), Times.Once);
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

    [Fact]
    public void IsValid_WithInvalidISBN_ReturnsFalse()
    {
        const string invalidISBN = "12345678";
        const string mockErrorMsg = "Invalid ISBN";
        // Arrange
        var book = new Book
        {
            Title = "Test Book",
            Author = "Test Author",
            ISBN = invalidISBN
        };
        _mockIsbnValidator.Setup(v => v.IsValid(invalidISBN)).Returns((false, mockErrorMsg));

        // Act
        var result = _sut.IsValid(book);

        // Assert
        Assert.False(result.isValid);
        Assert.Equal(mockErrorMsg, result.errorMessage);
        _mockIsbnValidator.Verify(v => v.IsValid(invalidISBN), Times.Once);
    }
}