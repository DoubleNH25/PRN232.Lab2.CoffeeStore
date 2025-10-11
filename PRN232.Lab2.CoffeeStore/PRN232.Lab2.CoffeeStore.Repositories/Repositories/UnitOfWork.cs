using PRN232.Lab2.CoffeeStore.Repositories.Contexts;
using PRN232.Lab2.CoffeeStore.Repositories.Entities;
using PRN232.Lab2.CoffeeStore.Repositories.Interfaces;

namespace PRN232.Lab2.CoffeeStore.Repositories.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CoffeeStoreDbContext _context;
    private IGenericRepository<Category>? _categories;
    private IGenericRepository<Product>? _products;
    private IGenericRepository<Order>? _orders;
    private IGenericRepository<OrderDetail>? _orderDetails;
    private IGenericRepository<Payment>? _payments;
    private IGenericRepository<RefreshToken>? _refreshTokens;

    public UnitOfWork(CoffeeStoreDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<Category> Categories =>
        _categories ??= new GenericRepository<Category>(_context);

    public IGenericRepository<Product> Products =>
        _products ??= new GenericRepository<Product>(_context);

    public IGenericRepository<Order> Orders =>
        _orders ??= new GenericRepository<Order>(_context);

    public IGenericRepository<OrderDetail> OrderDetails =>
        _orderDetails ??= new GenericRepository<OrderDetail>(_context);

    public IGenericRepository<Payment> Payments =>
        _payments ??= new GenericRepository<Payment>(_context);

    public IGenericRepository<RefreshToken> RefreshTokens =>
        _refreshTokens ??= new GenericRepository<RefreshToken>(_context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
