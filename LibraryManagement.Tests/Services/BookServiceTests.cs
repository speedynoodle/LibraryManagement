using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Services;
using Moq;

namespace Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _mockBookRepository;
    private readonly Mock<IValidator<Book>> _mockBookValidator;
    private readonly BookService _sut;
    
    public BookServiceTests()
    {
        _mockBookRepository = new Mock<IBookRepository>();
        _mockBookValidator = new Mock<IValidator<Book>>();
        
        _sut = new BookService(_mockBookRepository.Object, _mockBookValidator.Object);
    }
    
    [Fact]
    public async Task AddBook_NullBook_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.AddBook(null));
        Assert.Equal("Value cannot be null. (Parameter 'book')", exception.Message);
        _mockBookRepository.Verify(r => r.Add(It.IsAny<Book>()), Times.Never);
    }
    
    [Fact]
    public async Task AddBook_InvalidBook_ThrowsValidationException()
    {
        // Arrange
        const string expectedMessage = "Author is required";
        var book = new Book { Title = "Some Book", Author = ""};

        _mockBookValidator.Setup(v => v.IsValid(It.IsAny<Book>())).Returns((false, expectedMessage));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _sut.AddBook(book));
        Assert.Equal(expectedMessage, exception.Message);
        _mockBookRepository.Verify(r => r.Add(It.IsAny<Book>()), Times.Never);
    }
    
    [Fact]
    public async Task AddBook_ValidBook_ReturnsBookId()
    {
        // Arrange
        var book = new Book { Title = "Some Book", Author = "Some Author"};

        _mockBookValidator.Setup(v => v.IsValid(It.IsAny<Book>())).Returns((true, string.Empty));
        _mockBookRepository.Setup(r => r.Add(It.IsAny<Book>())).ReturnsAsync(book); 

        // Act
        var result = await _sut.AddBook(book);

        // Assert
        Assert.Equal(book.Id, result);
        _mockBookRepository.Verify(r => r.Add(It.IsAny<Book>()), Times.Once);
    }
}