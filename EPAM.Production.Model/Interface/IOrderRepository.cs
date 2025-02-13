using EPAM.Production.Repository.Model;

namespace EPAM.Production.Repository.Interface;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IReadOnlyList<Order>> GetFilteredOrdersAsync(int? month, int? year, Status? status, Guid? productId);

    Task DeleteFilteredOrdersAsync(int? month, int? year, Status? status, Guid? productId);
}
