using Dapper;
using EPAM.Production.Repository.Interface;
using EPAM.Production.Repository.Model;
using System.Data;

namespace EPAM.Production.Dapper.Repositories;

public class OrderRepositoryDapper : IOrderRepository
{
    private readonly IDbConnection _dbConnection;

    public OrderRepositoryDapper(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
    }

    public async Task<IReadOnlyList<Order>> GetAsync()
    {
        var query = "SELECT * FROM Orders";
        return (await _dbConnection.QueryAsync<Order>(query)).AsList();
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        var query = "SELECT * FROM Orders WHERE Id = @Id";
        return await _dbConnection.QueryFirstOrDefaultAsync<Order>(query, new { Id = id });
    }

    public async Task CreateAsync(Order entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var query = "INSERT INTO Orders (Id, CreatedDate, Status, ProductId) VALUES (@Id, @CreatedDate, @Status, @ProductId)";
        await _dbConnection.ExecuteAsync(query, entity);
    }

    public async Task UpdateAsync(Order entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var query = "UPDATE Orders SET CreatedDate = @CreatedDate, Status = @Status, ProductId = @ProductId WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(query, entity);
    }

    public async Task DeleteAsync(Order entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var query = "DELETE FROM Orders WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(query, new { entity.Id });
    }

    public async Task<IReadOnlyList<Order>> GetFilteredOrdersAsync(int? month, int? year, Status? status, Guid? productId)
    {
        var query = "SELECT * FROM Orders WHERE (MONTH(CreatedDate) = @Month OR @Month IS NULL) " +
                    "AND (YEAR(CreatedDate) = @Year OR @Year IS NULL) " +
                    "AND (Status = @Status OR @Status IS NULL) " +
                    "AND (ProductId = @ProductId OR @ProductId IS NULL)";

        return (await _dbConnection.QueryAsync<Order>(query, new { Month = month, Year = year, Status = status, ProductId = productId })).AsList();
    }

    public async Task DeleteFilteredOrdersAsync(int? month, int? year, Status? status, Guid? productId)
    {
        var query = "DELETE FROM Orders WHERE (MONTH(CreatedDate) = @Month OR @Month IS NULL) " +
                    "AND (YEAR(CreatedDate) = @Year OR @Year IS NULL) " +
                    "AND (Status = @Status OR @Status IS NULL) " +
                    "AND (ProductId = @ProductId OR @ProductId IS NULL)";

        await _dbConnection.ExecuteAsync(query, new { Month = month, Year = year, Status = status, ProductId = productId });
    }
}
