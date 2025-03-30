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
        var book = new Book { Title = "Some Book", Author = "Some Author", ISBN = "9780132350884"};

        _mockBookValidator.Setup(v => v.IsValid(It.IsAny<Book>())).Returns((true, string.Empty));
        _mockBookRepository.Setup(r => r.IsbnExists(book.ISBN, null)).ReturnsAsync(false);
        _mockBookRepository.Setup(r => r.Add(It.IsAny<Book>())).ReturnsAsync(book); 

        // Act
        var result = await _sut.AddBook(book);

        // Assert
        Assert.Equal(book.Id, result);
        _mockBookRepository.Verify(r => r.IsbnExists(book.ISBN, null), Times.Once);
        _mockBookRepository.Verify(r => r.Add(It.IsAny<Book>()), Times.Once);
    }
    
    [Fact]
    public async Task AddBook_DuplicateISBN_ThrowsValidationException()
    {
        // Arrange
        var book = new Book { Title = "Some Book", Author = "Some Author", ISBN = "9780132350884"};
        const string expectedMessage = "ISBN is already in use by another book";

        _mockBookValidator.Setup(v => v.IsValid(It.IsAny<Book>())).Returns((true, string.Empty));
        _mockBookRepository.Setup(r => r.IsbnExists(book.ISBN, null)).ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _sut.AddBook(book));
        Assert.Equal(expectedMessage, exception.Message);
        _mockBookRepository.Verify(r => r.IsbnExists(book.ISBN, null), Times.Once);
        _mockBookRepository.Verify(r => r.Add(It.IsAny<Book>()), Times.Never);
    }
    
    [Fact]
    public async Task GetBookById_CallsBookRepositoryGetById()
    {  
        // Arrange
        const int someBookId = 1;
        
        // Act
        _ = await _sut.GetBookById(someBookId);
        
        // Assert
        _mockBookRepository.Verify(r => r.GetById(someBookId), Times.Once);
    }
    
    [Fact]
    public async Task GetAllBooks_CallsBookRepositoryGetById()
    {  
        // Arrange
        // Act
        _ = await _sut.GetAllBooks();
        
        // Assert
        _mockBookRepository.Verify(r => r.GetAll(), Times.Once);
    }
    
    [Fact]
    public async Task UpdateBook_WithValidBook_ReturnsTrue()
    {
        // Arrange
        var book = new Book
        {
            Id = 1,
            Title = "Updated Title",
            Author = "Updated Author",
            ISBN = "9780132350884"
        };

        _mockBookRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(book);
        _mockBookValidator.Setup(v => v.IsValid(It.IsAny<Book>())).Returns((true, string.Empty));
        _mockBookRepository.Setup(repo => repo.IsbnExists(book.ISBN, book.Id)).ReturnsAsync(false);

        // Act
        var result =  await _sut.UpdateBook(book);

        // Assert
        Assert.True(result);
        _mockBookRepository.Verify(repo => repo.IsbnExists(book.ISBN, book.Id), Times.Once);
        _mockBookRepository.Verify(repo => repo.Update(It.IsAny<Book>()), Times.Once);
        _mockBookValidator.Verify(v => v.IsValid(It.IsAny<Book>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateBook_WithDuplicateISBN_ThrowsValidationException()
    {
        // Arrange
        var book = new Book
        {
            Id = 1,
            Title = "Updated Title",
            Author = "Updated Author",
            ISBN = "9780132350884"
        };
        const string expectedMessage = "ISBN is already in use by another book";

        _mockBookValidator.Setup(v => v.IsValid(It.IsAny<Book>())).Returns((true, string.Empty));
        _mockBookRepository.Setup(repo => repo.IsbnExists(book.ISBN, book.Id)).ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _sut.UpdateBook(book));
        Assert.Equal(expectedMessage, exception.Message);
        _mockBookRepository.Verify(repo => repo.IsbnExists(book.ISBN, book.Id), Times.Once);
        _mockBookRepository.Verify(repo => repo.Update(It.IsAny<Book>()), Times.Never);
    }
    
    [Fact]
    public async Task DeleteBook_WithValidId_ReturnsTrue()
    {
        // Arrange
        var book = new Book { Title = "Some Book", Author = "Some Author"};
        _mockBookRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(book);
        
        // Act1
        
        var result = await _sut.DeleteBook(1);

        // Assert
        Assert.True(result);
        _mockBookRepository.Verify(repo => repo.Remove(book), Times.Once);
    }
}