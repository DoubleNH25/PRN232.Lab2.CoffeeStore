using PRN232.Lab2.CoffeeStore.Repositories.Entities;

namespace PRN232.Lab2.CoffeeStore.Repositories.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    IGenericRepository<Category> Categories { get; }
    IGenericRepository<Product> Products { get; }
    IGenericRepository<Order> Orders { get; }
    IGenericRepository<OrderDetail> OrderDetails { get; }
    IGenericRepository<Payment> Payments { get; }
    IGenericRepository<RefreshToken> RefreshTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
