using Dapper;
using EPAM.Production.Dapper.Repositories;
using EPAM.Production.Repository.Model;
using Moq;
using Moq.Dapper;
using System.Data;

namespace EPAM.Production.Tests.Dapper;

public class ProductRepositoryTests
{
    private Mock<IDbConnection> _mockDbConnection;
    private ProductRepositoryDapper _repository;

    [SetUp]
    public void Setup()
    {
        _mockDbConnection = new Mock<IDbConnection>();
        _repository = new ProductRepositoryDapper(_mockDbConnection.Object);
    }

    [Test]
    public async Task GetAsync_ShouldReturnListOfProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name = "Product A" },
            new Product { Id = Guid.NewGuid(), Name = "Product B" }
        };

        _mockDbConnection.SetupDapperAsync(c => c.QueryAsync<Product>(It.IsAny<string>(), null, null, null, null))
                         .ReturnsAsync(products);

        // Act
        var result = await _repository.GetAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        });
    }
}