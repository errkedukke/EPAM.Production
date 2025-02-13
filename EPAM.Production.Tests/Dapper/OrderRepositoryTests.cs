using Dapper;
using EPAM.Production.Dapper.Repositories;
using EPAM.Production.Repository.Model;
using Moq;
using Moq.Dapper;
using System.Data;

namespace EPAM.Production.Tests.Dapper;

public class OrderRepositoryTests
{
    private Mock<IDbConnection> _mockDbConnection;
    private OrderRepositoryDapper _repository;

    [SetUp]
    public void Setup()
    {
        _mockDbConnection = new Mock<IDbConnection>();
        _repository = new OrderRepositoryDapper(_mockDbConnection.Object);
    }

    [Test]
    public async Task GetAsync_ShouldReturnListOfOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, Status = Status.Done, ProductId = Guid.NewGuid() },
            new Order { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, Status = Status.Arrived, ProductId = Guid.NewGuid() }
        };

        _mockDbConnection.SetupDapperAsync(c => c.QueryAsync<Order>(It.IsAny<string>(), null, null, null, null))
                         .ReturnsAsync(orders);

        // Act
        var result = await _repository.GetAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        });
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnOrder_WhenOrderExists()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Order { Id = orderId, CreatedDate = DateTime.UtcNow, Status = Status.Cancelled, ProductId = Guid.NewGuid() };

        _mockDbConnection.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<Order>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                         .ReturnsAsync(order);

        // Act
        var result = await _repository.GetByIdAsync(orderId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(orderId));
    }
}