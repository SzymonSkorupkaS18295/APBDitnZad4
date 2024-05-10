namespace APBDcw4.Warehouse.DBContext;

using Microsoft.EntityFrameworkCore;

public class WarehouseDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product_Warehouse> Product_Warehouses { get; set; }

    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToTable("Product");
        modelBuilder.Entity<Warehouse>().ToTable("Warehouse");
        modelBuilder.Entity<Order>().ToTable("Order");
        modelBuilder.Entity<Product_Warehouse>().ToTable("Product_Warehouse");
    }
}
