namespace LibraryManagement.Core.Interfaces;

/// <summary>
/// Generic repository interface defining standard operations for an entity
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public interface IRepository<T> where T : class
{
    Task<T?> Add(T entity);
    Task<T?> GetById(int id);
    Task<IEnumerable<T>> GetAll();
    Task Update(T entity);
    Task Remove(T entity);
}