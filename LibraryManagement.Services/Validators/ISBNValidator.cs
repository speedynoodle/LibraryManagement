using System.Text.RegularExpressions;
using LibraryManagement.Core.Interfaces;
using static System.Char;

namespace LibraryManagement.Services.Validators;

public class ISBNValidator : IValidator<string>
{
    private static readonly Regex IsbnTenCharRegex = new Regex(@"^\d{9}[\dXx]$");
    private static readonly Regex IsbnThirteenCharRegex = new Regex(@"^\d{13}$");
   
    private const string DefaultInvalidIsbnErrorMsg = "ISBN is invalid";

    public (bool isValid, string? errorMessage) IsValid(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
        {
            return (false, "ISBN is required");
        }

        var cleanIsbn = isbn.Replace("-", "").Replace(" ", "").Trim();

        // Check if it's ISBN-10
        if (IsbnTenCharRegex.IsMatch(cleanIsbn))
        {
            return ValidateTenCharFormat(cleanIsbn);
        }
            
        // Check if it's ISBN-13
        if (IsbnThirteenCharRegex.IsMatch(cleanIsbn))
        {
            return ValidateThirteenCharFormat(cleanIsbn);
        }
        
        return (false, DefaultInvalidIsbnErrorMsg);

    }
    
    private (bool isValid, string? errorMessage) ValidateTenCharFormat(string isbn)
    {
        //weight the first 9 characters
        var totalWeightedSum = 0;
        for (var i = 0; i < 9; i++)
        {
            totalWeightedSum += (int)GetNumericValue(isbn[i]) * (10 - i);
        }
        
        // Check the last character, which can be 'X' representing 10
        var lastChar = isbn[9];
        if (ToLower(lastChar) == 'x')
        {
            totalWeightedSum += 10;
        }
        else
        {
            totalWeightedSum += (int)GetNumericValue(lastChar);
        }

        return totalWeightedSum % 11 == 0 
            ? (true, null) 
            : (false, DefaultInvalidIsbnErrorMsg);
    }

    private (bool isValid, string? errorMessage) ValidateThirteenCharFormat(string isbn)
    {
        //weight the first 12 characters
        var totalWeightedSum = 0;
        for (var i = 0; i < 12; i++)
        {
            totalWeightedSum += (int)GetNumericValue(isbn[i]) * (i % 2 == 0 ? 1 : 3); //multiple odds by 1, and evens by 3
        }
        
        var checksumDigit = (10 - (totalWeightedSum % 10)) % 10;
        
        return checksumDigit == (int)GetNumericValue(isbn[12])
            ? (true, null) 
            : (false, DefaultInvalidIsbnErrorMsg);
    }
}