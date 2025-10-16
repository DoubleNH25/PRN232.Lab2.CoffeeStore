using PRN232.Lab2.CoffeeStore.Repositories.Contexts;
using PRN232.Lab2.CoffeeStore.Repositories.Interfaces;
using PRN232.Lab2.CoffeeStore.Repositories.Models;
using PRN232.Lab2.CoffeeStore.Repositories.Repositories;

namespace PRN232.Lab2.CoffeeStore.Repositories.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoffeeStoreDbContext _context;

        private IRepository<Category>? _categoryRepository;
        private IRepository<Product>? _productRepository;
        private IRepository<User>? _userRepository;
        private IRepository<Order>? _orderRepository;
        private IRepository<OrderDetail>? _orderDetailRepository;
        private IRepository<Payment>? _paymentRepository;
        private IRepository<RefreshToken>? _refreshTokenRepository;

        public UnitOfWork(CoffeeStoreDbContext context)
        {
            _context = context;
        }

        public IRepository<Category> CategoryRepository => 
            _categoryRepository ??= new GenericRepository<Category>(_context);

        public IRepository<Product> ProductRepository => 
            _productRepository ??= new GenericRepository<Product>(_context);

        public IRepository<User> UserRepository => 
            _userRepository ??= new GenericRepository<User>(_context);

        public IRepository<Order> OrderRepository => 
            _orderRepository ??= new GenericRepository<Order>(_context);

        public IRepository<OrderDetail> OrderDetailRepository => 
            _orderDetailRepository ??= new GenericRepository<OrderDetail>(_context);

        public IRepository<Payment> PaymentRepository => 
            _paymentRepository ??= new GenericRepository<Payment>(_context);

        public IRepository<RefreshToken> RefreshTokenRepository => 
            _refreshTokenRepository ??= new GenericRepository<RefreshToken>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}