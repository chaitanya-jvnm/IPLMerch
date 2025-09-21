using IPLMerch.Application.Models;
using IPLMerch.Enums;
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

        // Seed Data
        SeedData(modelBuilder);

        base.OnModelCreating(modelBuilder);
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

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Users
        var users = new[]
        {
            new User
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                FirstName = "Default",
                LastName = "User",
                Email = "defaultuser@email.com",
                PhoneNumber = "9999999999"
            }
        };

        modelBuilder.Entity<User>().HasData(users);
        
        // Seed Franchises
        var franchises = new[]
        {
            new Franchise
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Mumbai Indians", Code = "MI",
                City = "Mumbai"
            },
            new Franchise
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Chennai Super Kings", Code = "CSK",
                City = "Chennai"
            },
            new Franchise
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Royal Challengers Bangalore",
                Code = "RCB", City = "Bangalore"
            },
            new Franchise
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Kolkata Knight Riders", Code = "KKR",
                City = "Kolkata"
            }
        };

        modelBuilder.Entity<Franchise>().HasData(franchises);

        // Seed Products
        var productId = 1;
        var products = franchises.SelectMany(f => new[]
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = $"{f.Name} Official Jersey 2024",
                Type = ProductType.Jersey,
                Price = 2999,
                FranchiseId = f.Id,
                SKU = $"{f.Code}-JER-{productId++:D4}",
                StockQuantity = 100,
                IsAvailable = true,
                Description = $"Official {f.Name} jersey for IPL 2024 season"
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = $"{f.Name} Team Cap",
                Type = ProductType.Cap,
                Price = 499,
                FranchiseId = f.Id,
                SKU = $"{f.Code}-CAP-{productId++:D4}",
                StockQuantity = 200,
                IsAvailable = true,
                Description = $"Official {f.Name} team cap with logo"
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = $"{f.Name} Victory Flag",
                Type = ProductType.Flag,
                Price = 299,
                FranchiseId = f.Id,
                SKU = $"{f.Code}-FLG-{productId++:D4}",
                StockQuantity = 150,
                IsAvailable = true,
                Description = $"{f.Name} team flag for true fans"
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = $"{f.Name} Captain's Bat",
                Type = ProductType.SportingGear,
                Price = 4999,
                FranchiseId = f.Id,
                SKU = $"{f.Code}-APH-{productId++:D4}",
                StockQuantity = 10,
                IsAvailable = true,
                IsAutographed = true,
                Description = $"Bat of {f.Name} captain"
            }
        });

        modelBuilder.Entity<Product>().HasData(products);
    }
}