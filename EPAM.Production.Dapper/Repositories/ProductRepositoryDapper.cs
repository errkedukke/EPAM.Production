using Dapper;
using EPAM.Production.Repository.Interface;
using EPAM.Production.Repository.Model;
using System.Data;

namespace EPAM.Production.Dapper.Repositories;

public class ProductRepositoryDapper : IProductRepository
{
    private readonly IDbConnection _dbConnection;

    public ProductRepositoryDapper(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
    }

    public async Task<IReadOnlyList<Product>> GetAsync()
    {
        var query = "SELECT * FROM Products";
        return (await _dbConnection.QueryAsync<Product>(query)).AsList();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        var query = "SELECT * FROM Products WHERE Id = @Id";
        return await _dbConnection.QueryFirstOrDefaultAsync<Product>(query, new { Id = id });
    }

    public async Task CreateAsync(Product entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var query = "INSERT INTO Products (Id, Name, Price) VALUES (@Id, @Name, @Price)";
        await _dbConnection.ExecuteAsync(query, entity);
    }

    public async Task UpdateAsync(Product entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var query = "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(query, entity);
    }

    public async Task DeleteAsync(Product entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var query = "DELETE FROM Products WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(query, new { entity.Id });
    }
}
