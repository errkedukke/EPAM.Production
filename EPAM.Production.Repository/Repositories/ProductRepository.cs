using EPAM.Production.Repository.DbContext;
using EPAM.Production.Repository.Interface;
using EPAM.Production.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace EPAM.Production.Repository.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductionDbContext _context;

    public ProductRepository(ProductionDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IReadOnlyList<Product>> GetAsync()
    {
        return await _context.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task CreateAsync(Product entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _context.Products.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Products.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
    }
}