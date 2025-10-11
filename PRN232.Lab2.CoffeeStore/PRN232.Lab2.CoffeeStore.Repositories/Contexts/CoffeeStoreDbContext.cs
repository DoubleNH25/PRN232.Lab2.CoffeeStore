using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PRN232.Lab2.CoffeeStore.Repositories.Entities;

namespace PRN232.Lab2.CoffeeStore.Repositories.Contexts;

public class CoffeeStoreDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public CoffeeStoreDbContext(DbContextOptions<CoffeeStoreDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FullName).HasMaxLength(200);
        });

        builder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");
            entity.HasKey(e => e.CategoryId);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        builder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");
            entity.HasKey(e => e.ProductId);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.HasOne(e => e.Category)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");
            entity.HasKey(e => e.OrderId);
            entity.Property(e => e.Status).HasMaxLength(50).IsRequired();
            entity.Property(e => e.OrderDate).HasColumnType("datetime2");
            entity.HasOne<Payment>(e => e.Payment)
                .WithOne(e => e.Order)
                .HasForeignKey<Payment>(e => e.OrderId);
        });

        builder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");
            entity.HasKey(e => e.OrderDetailId);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
            entity.HasOne(e => e.Order)
                .WithMany(e => e.OrderDetails)
                .HasForeignKey(e => e.OrderId);
            entity.HasOne(e => e.Product)
                .WithMany(e => e.OrderDetails)
                .HasForeignKey(e => e.ProductId);
        });

        builder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");
            entity.HasKey(e => e.PaymentId);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime2");
            entity.Property(e => e.PaymentMethod).HasMaxLength(100).IsRequired();
        });

        builder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshToken");
            entity.HasKey(e => e.RefreshTokenId);
            entity.Property(e => e.Token).HasMaxLength(512).IsRequired();
            entity.Property(e => e.UserId).HasMaxLength(450).IsRequired();
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime2");
            entity.HasIndex(e => e.Token).IsUnique();
        });

        var adminUserId = "2AA19EFC-1797-4BE3-9A0B-54BFA4ABF722";
        var staffUserId = "C0A9F126-67E7-4B42-A85B-1B9D6685C3BD";

        var initialCreated = new DateTime(2024, 1, 1);

        var adminUser = new ApplicationUser
        {
            Id = adminUserId,
            UserName = "admin@coffeestore.com",
            NormalizedUserName = "ADMIN@COFFEESTORE.COM",
            Email = "admin@coffeestore.com",
            NormalizedEmail = "ADMIN@COFFEESTORE.COM",
            EmailConfirmed = true,
            FullName = "Coffee Store Admin",
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            CreatedDate = initialCreated,
            UpdatedDate = null
        };

        var staffUser = new ApplicationUser
        {
            Id = staffUserId,
            UserName = "staff@coffeestore.com",
            NormalizedUserName = "STAFF@COFFEESTORE.COM",
            Email = "staff@coffeestore.com",
            NormalizedEmail = "STAFF@COFFEESTORE.COM",
            EmailConfirmed = true,
            FullName = "Coffee Store Staff",
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            CreatedDate = initialCreated.AddDays(1),
            UpdatedDate = null
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123");
        staffUser.PasswordHash = passwordHasher.HashPassword(staffUser, "Staff@123");

        builder.Entity<ApplicationUser>().HasData(adminUser, staffUser);

        var categories = new List<Category>();
        for (var i = 1; i <= 20; i++)
        {
            categories.Add(new Category
            {
                CategoryId = i,
                Name = $"Category {i}",
                Description = $"Category {i} description",
                CreatedDate = new DateTime(2024, 1, 1).AddDays(i),
                UpdatedDate = null
            });
        }
        builder.Entity<Category>().HasData(categories);

        var products = new List<Product>();
        var productId = 1;
        foreach (var category in categories)
        {
            for (var i = 1; i <= 4; i++)
            {
                products.Add(new Product
                {
                    ProductId = productId,
                    Name = $"Product {productId}",
                    Description = $"Product {productId} description",
                    Price = 3.5m + productId,
                    IsActive = true,
                    CategoryId = category.CategoryId,
                    CreatedDate = new DateTime(2024, 2, 1).AddDays(productId),
                    UpdatedDate = null
                });
                productId++;
            }
        }
        builder.Entity<Product>().HasData(products);

        var orders = new List<Order>();
        for (var i = 1; i <= 20; i++)
        {
            orders.Add(new Order
            {
                OrderId = i,
                UserId = i % 2 == 0 ? adminUserId : staffUserId,
                OrderDate = new DateTime(2024, 3, 1).AddDays(i),
                Status = (i % 4) switch
                {
                    0 => "Completed",
                    1 => "Processing",
                    2 => "Pending",
                    _ => "Shipped"
                },
                PaymentId = i,
                CreatedDate = new DateTime(2024, 3, 1).AddDays(i),
                UpdatedDate = null
            });
        }
        builder.Entity<Order>().HasData(orders);

        var payments = new List<Payment>();
        for (var i = 1; i <= 20; i++)
        {
            payments.Add(new Payment
            {
                PaymentId = i,
                OrderId = i,
                Amount = 25m + i * 5,
                PaymentDate = new DateTime(2024, 3, 5).AddDays(i),
                PaymentMethod = i % 2 == 0 ? "Credit Card" : "Cash",
                CreatedDate = new DateTime(2024, 3, 5).AddDays(i),
                UpdatedDate = null
            });
        }
        builder.Entity<Payment>().HasData(payments);

        var orderDetails = new List<OrderDetail>();
        var orderDetailId = 1;
        foreach (var order in orders)
        {
            var categoryIndex = ((order.OrderId - 1) % categories.Count) + 1;
            var details = products
                .Where(p => p.CategoryId == categoryIndex)
                .Take(2)
                .ToList();

            for (var i = 0; i < details.Count; i++)
            {
                orderDetails.Add(new OrderDetail
                {
                    OrderDetailId = orderDetailId,
                    OrderId = order.OrderId,
                    ProductId = details[i].ProductId,
                    Quantity = 1 + ((order.OrderId + i) % 3),
                    UnitPrice = details[i].Price,
                    CreatedDate = new DateTime(2024, 3, 10).AddDays(orderDetailId),
                    UpdatedDate = null
                });
                orderDetailId++;
            }
        }
        builder.Entity<OrderDetail>().HasData(orderDetails);

        var refreshTokens = new List<RefreshToken>();
        for (var i = 1; i <= 20; i++)
        {
            refreshTokens.Add(new RefreshToken
            {
                RefreshTokenId = i,
                Token = $"seed-refresh-token-{i}",
                UserId = i % 2 == 0 ? adminUserId : staffUserId,
                ExpiryDate = new DateTime(2024, 4, 1).AddDays(i),
                IsRevoked = false,
                CreatedDate = new DateTime(2024, 3, 15).AddDays(i),
                UpdatedDate = null
            });
        }
        builder.Entity<RefreshToken>().HasData(refreshTokens);
    }
}
