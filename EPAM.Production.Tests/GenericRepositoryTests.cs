using EPAM.Production.Repository.DbContext;
using EPAM.Production.Repository.Model;
using EPAM.Production.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EPAM.Production.Tests
{
    public class GenericRepositoryTests
    {
        private ProductionDbContext _context;
        private GenericRepository<Product> _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ProductionDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ProductionDbContext(options);
            _repository = new GenericRepository<Product>(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateAsync_ShouldAddEntity()
        {
            // Arrange
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Weight = 1.5m,
                Height = 2.0m,
                Width = 3.0m,
                Length = 4.0m
            };

            // Act
            await _repository.CreateAsync(product);
            var result = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null, "Result should not be null.");
                Assert.That(result.Name, Is.EqualTo(product.Name), "Name should match.");
                Assert.That(result.Description, Is.EqualTo(product.Description), "Description should match.");
            });
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectEntity()
        {
            // Arrange
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Weight = 1.5m
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(product.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null, "Result should not be null.");
                Assert.That(result.Id, Is.EqualTo(product.Id), "ID should match.");
                Assert.That(result.Name, Is.EqualTo(product.Name), "Name should match.");
            });
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            // Arrange
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product"
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(product);
            var result = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);

            // Assert
            Assert.That(result, Is.Null, "Entity should be null after deletion.");
        }
    }
}
