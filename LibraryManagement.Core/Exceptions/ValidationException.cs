namespace LibraryManagement.Core.Exceptions;

public class ValidationException : Exception
{
    private const string DefaultMessage = "Validation failed. Please check the input.";

    public ValidationException(string message) : base(message ?? DefaultMessage)
    {
    }
}