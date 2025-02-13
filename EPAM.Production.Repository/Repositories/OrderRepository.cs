using EPAM.Production.Repository.DbContext;
using EPAM.Production.Repository.Interface;
using EPAM.Production.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace EPAM.Production.Repository.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ProductionDbContext _context;

    public OrderRepository(ProductionDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IReadOnlyList<Order>> GetAsync()
    {
        return await _context.Orders.AsNoTracking().ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task CreateAsync(Order entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _context.Orders.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Orders.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Order entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Orders.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Order>> GetFilteredOrdersAsync(int? month, int? year, Status? status, Guid? productId)
    {
        var query = _context.Orders.AsQueryable();

        if (month.HasValue)
            query = query.Where(o => o.CreatedDate.Month == month.Value);

        if (year.HasValue)
            query = query.Where(o => o.CreatedDate.Year == year.Value);

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        if (productId.HasValue)
            query = query.Where(o => o.ProductId == productId.Value);

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task DeleteFilteredOrdersAsync(int? month, int? year, Status? status, Guid? productId)
    {
        var ordersToDelete = await GetFilteredOrdersAsync(month, year, status, productId);

        foreach (var order in ordersToDelete)
        {
            var trackedEntity = await _context.Orders.FindAsync(order.Id);

            if (trackedEntity != null)
            {
                _context.Orders.Remove(trackedEntity);
            }
            else
            {
                _context.Orders.Attach(order);
                _context.Orders.Remove(order);
            }
        }

        await _context.SaveChangesAsync();
    }
}