using EPAM.Production.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace EPAM.Production.Repository.DbContext;

public class ProductionDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ProductionDbContext(DbContextOptions<ProductionDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Description).IsRequired().HasMaxLength(500);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.CreatedDate).IsRequired();
            entity.HasOne(o => o.Product)
                  .WithMany()
                  .HasForeignKey(o => o.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
