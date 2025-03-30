using LibraryManagement.Services.Validators;

namespace Tests.Validators;


public class ISBNValidatorTests
{
    private readonly ISBNValidator _sut;


    public ISBNValidatorTests()
    {

        _sut = new ISBNValidator();
    }

    [Theory]
    [InlineData("9780132350884", true)] // Valid ISBN-13
    [InlineData("978-0-13-235088-4", true)] // Valid ISBN-13 with hyphens
    [InlineData("0132350882", true)] // Valid ISBN-10
    [InlineData("0-13-235088-2", true)] // Valid ISBN-10 with hyphens
    [InlineData("0201633612", true)] // Valid ISBN-10 with check digit X
    [InlineData("123456789X", true)] // Valid ISBN-10 with check digit X
    [InlineData("123456789", false)] // Too short
    [InlineData("12345678901234", false)] // Too long
    [InlineData("123456789Y", false)] // Invalid check character
    [InlineData("9780132350883", false)] // Invalid ISBN-13 check digit
    [InlineData("", false)] // Empty string
    [InlineData(null, false)] // Null
    public void IsValid_ShouldValidateCorrectly(string isbn, bool expected)
    {
        // Act
        var result = _sut.IsValid(isbn);

        // Assert
        Assert.Equal(expected, result.isValid);
    }
}
