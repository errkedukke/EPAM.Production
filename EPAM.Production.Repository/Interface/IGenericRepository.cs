namespace EPAM.Production.Repository.Interface;

public interface IGenericRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetAsync();

    Task<T> GetByIdAsync(Guid id);

    Task CreateAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);
}
