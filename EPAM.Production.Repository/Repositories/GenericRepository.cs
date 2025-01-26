using EPAM.Production.Repository.DbContext;
using EPAM.Production.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EPAM.Production.Repository.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ProductionDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ProductionDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IReadOnlyList<T>> GetAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException("Entity not found");
    }

    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
