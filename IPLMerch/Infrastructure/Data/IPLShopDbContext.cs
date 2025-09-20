using IPLMerch.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace IPLMerch.Infrastructure.Data;

public class IPLShopDbContext : DbContext
{
    public IPLShopDbContext(DbContextOptions<IPLShopDbContext> options)
        : base(options)
    {
    }

    public DbSet<Franchise> Franchises { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Franchise configuration
        modelBuilder.Entity<Franchise>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(10);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        // Product configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.SKU).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.SKU).IsUnique();
            entity.HasOne(e => e.Franchise)
                .WithMany(f => f.Products)
                .HasForeignKey(e => e.FranchiseId);
        });

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Cart configuration
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(e => e.UserId);
            entity.Ignore(e => e.TotalAmount);
        });

        // CartItem configuration
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(e => e.CartId);
            entity.HasOne(e => e.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(e => e.ProductId);
        });

        // Order configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.OrderNumber).IsUnique();
            entity.HasOne(e => e.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(e => e.UserId);
        });

        // OrderItem configuration
        // modelBuilder.Entity<OrderItem>(entity =>
        // {
        //     entity.HasKey(e => e.Id);
        //     entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
        //     entity.Property(e => e.TotalPrice).HasPrecision(18, 2);
        //     entity.HasOne(e => e.Order)
        //         .WithMany(o => o.Items)
        //         .HasForeignKey(e => e.OrderId);
        //     entity.HasOne(e => e.Product)
        //         .WithMany(p => p.OrderItems)
        //         .HasForeignKey(e => e.ProductId);
        // });

        // Seed data
        // SeedData(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added ||
                e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;
            entity.UpdatedAt = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}