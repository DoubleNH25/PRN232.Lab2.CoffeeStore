using PRN232.Lab2.CoffeeStore.Repositories.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;
using PRN232.Lab2.CoffeeStore.Services.Services;
using PRN232.Lab2.CoffeeStore.Repositories.Models;
using System.Linq.Dynamic.Core;

namespace PRN232.Lab2.CoffeeStore.Services.Services
{
    public class OrderService : BaseService, IOrderService
    {
        public OrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<OrderResponseModel>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync();
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var userDict = users.ToDictionary(u => u.UserId, u => $"{u.FirstName} {u.LastName}");

            return orders.Select(o => new OrderResponseModel
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                UserName = userDict.ContainsKey(o.UserId) ? userDict[o.UserId] : null
            });
        }

        public async Task<OrderResponseModel?> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null) return null;

            string? userName = null;
            var user = await _unitOfWork.UserRepository.GetByIdAsync(order.UserId);
            if (user != null)
            {
                userName = $"{user.FirstName} {user.LastName}";
            }

            return new OrderResponseModel
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                UserName = userName
            };
        }

        public async Task<OrderResponseModel> CreateOrderAsync(OrderRequestModel orderRequest)
        {
            var order = new Order
            {
                UserId = orderRequest.UserId,
                OrderDate = orderRequest.OrderDate ?? DateTime.Now,
                Status = orderRequest.Status ?? "Pending",
                TotalAmount = orderRequest.TotalAmount ?? 0
            };

            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            string? userName = null;
            var user = await _unitOfWork.UserRepository.GetByIdAsync(order.UserId);
            if (user != null)
            {
                userName = $"{user.FirstName} {user.LastName}";
            }

            return new OrderResponseModel
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                UserName = userName
            };
        }

        public async Task<OrderResponseModel?> UpdateOrderAsync(int id, OrderRequestModel orderRequest)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null) return null;

            order.UserId = orderRequest.UserId;
            order.OrderDate = orderRequest.OrderDate ?? order.OrderDate;
            order.Status = orderRequest.Status ?? order.Status;
            order.TotalAmount = orderRequest.TotalAmount ?? order.TotalAmount;

            _unitOfWork.OrderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync();

            string? userName = null;
            var user = await _unitOfWork.UserRepository.GetByIdAsync(order.UserId);
            if (user != null)
            {
                userName = $"{user.FirstName} {user.LastName}";
            }

            return new OrderResponseModel
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                UserName = userName
            };
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null) return false;

            _unitOfWork.OrderRepository.Remove(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<OrderResponseModel> Orders, int TotalCount)> GetOrdersAsync(
            string? search = null,
            string? sortBy = null,
            bool ascending = true,
            int page = 1,
            int pageSize = 10,
            string? fields = null,
            int? userId = null)
        {
            // Get all orders
            var allOrders = (await _unitOfWork.OrderRepository.GetAllAsync()).AsQueryable();
            
            // Include users for user name
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var userDict = users.ToDictionary(u => u.UserId, u => $"{u.FirstName} {u.LastName}");

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                allOrders = allOrders.Where(o => o.Status.Contains(search));
            }

            // Apply user filter
            if (userId.HasValue)
            {
                allOrders = allOrders.Where(o => o.UserId == userId);
            }

            // Get total count before pagination
            var totalCount = allOrders.Count();

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                // Handle special case for UserName
                if (sortBy.Equals("userName", StringComparison.OrdinalIgnoreCase))
                {
                    var sortDirection = ascending ? "asc" : "desc";
                    allOrders = allOrders.OrderBy(o => userDict.ContainsKey(o.UserId) ? userDict[o.UserId] : "").ThenBy(o => o.OrderId);
                    if (!ascending)
                    {
                        allOrders = allOrders.Reverse();
                    }
                }
                else
                {
                    var sortDirection = ascending ? "asc" : "desc";
                    allOrders = allOrders.OrderBy($"{sortBy} {sortDirection}");
                }
            }
            else
            {
                allOrders = allOrders.OrderBy(o => o.OrderId);
            }

            // Apply pagination
            var orders = allOrders.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Map to response models
            var orderResponses = orders.Select(o => new OrderResponseModel
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                UserName = userDict.ContainsKey(o.UserId) ? userDict[o.UserId] : null
            });

            return (orderResponses, totalCount);
        }
    }
}