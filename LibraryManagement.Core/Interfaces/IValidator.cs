namespace LibraryManagement.Core.Interfaces;

public interface IValidator<in T>
{
    (bool isValid, string? errorMessage) IsValid(T entity);
}