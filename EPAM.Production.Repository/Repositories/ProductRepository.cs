using EPAM.Production.Repository.DbContext;
using EPAM.Production.Repository.Interface;
using EPAM.Production.Repository.Model;

namespace EPAM.Production.Repository.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ProductionDbContext context) : base(context) { }
}
