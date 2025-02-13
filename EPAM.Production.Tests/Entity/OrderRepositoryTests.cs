using EPAM.Production.Repository.DbContext;
using EPAM.Production.Repository.Model;
using EPAM.Production.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EPAM.Production.Tests.Entity;

public class OrderRepositoryTests
{
    private ProductionDbContext _context;
    private OrderRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ProductionDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ProductionDbContext(options);
        _repository = new OrderRepository(_context);
    }

    [TearDown]
    public void Teardown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetFilteredOrdersAsync_ShouldReturnFilteredResults()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var order1 = new Order
        {
            Id = Guid.NewGuid(),
            CreatedDate = new DateTime(2023, 1, 1),
            Status = Status.Arrived,
            ProductId = productId
        };
        var order2 = new Order
        {
            Id = Guid.NewGuid(),
            CreatedDate = new DateTime(2023, 2, 1),
            Status = Status.Loading,
            ProductId = productId
        };

        await _context.Orders.AddRangeAsync(order1, order2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetFilteredOrdersAsync(1, 2023, Status.Arrived, productId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Id, Is.EqualTo(order1.Id));
        });
    }

    [Test]
    public async Task DeleteFilteredOrdersAsync_ShouldRemoveFilteredResults()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var order1 = new Order
        {
            Id = Guid.NewGuid(),
            CreatedDate = new DateTime(2023, 1, 1),
            Status = Status.Loading,
            ProductId = productId
        };
        var order2 = new Order
        {
            Id = Guid.NewGuid(),
            CreatedDate = new DateTime(2023, 2, 1),
            Status = Status.Cancelled,
            ProductId = productId
        };

        await _context.Orders.AddRangeAsync(order1, order2);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteFilteredOrdersAsync(1, 2023, Status.Loading, productId);

        var remainingOrders = await _context.Orders.ToListAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(remainingOrders, Is.Not.Null);
            Assert.That(remainingOrders.Count, Is.EqualTo(1));
            Assert.That(remainingOrders[0].Id, Is.EqualTo(order2.Id));
        });
    }
}
