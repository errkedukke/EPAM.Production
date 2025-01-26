using EPAM.Production.Repository.DbContext;
using EPAM.Production.Repository.Interface;
using EPAM.Production.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace EPAM.Production.Repository.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ProductionDbContext context) : base(context) { }

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

            return await query.ToListAsync();
        }

        public async Task DeleteFilteredOrdersAsync(int? month, int? year, Status? status, Guid? productId)
        {
            var ordersToDelete = await GetFilteredOrdersAsync(month, year, status, productId);

            _context.Orders.RemoveRange(ordersToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
