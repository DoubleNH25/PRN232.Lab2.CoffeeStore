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
    public class OrderDetailService : BaseService, IOrderDetailService
    {
        public OrderDetailService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<OrderDetailResponseModel>> GetAllOrderDetailsAsync()
        {
            var orderDetails = await _unitOfWork.OrderDetailRepository.GetAllAsync();
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            var productDict = products.ToDictionary(p => p.ProductId, p => p.Name);

            return orderDetails.Select(od => new OrderDetailResponseModel
            {
                OrderDetailId = od.OrderDetailId,
                OrderId = od.OrderId,
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
                ProductName = productDict.ContainsKey(od.ProductId) ? productDict[od.ProductId] : null
            });
        }

        public async Task<OrderDetailResponseModel?> GetOrderDetailByIdAsync(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
            if (orderDetail == null) return null;

            string? productName = null;
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(orderDetail.ProductId);
            if (product != null)
            {
                productName = product.Name;
            }

            return new OrderDetailResponseModel
            {
                OrderDetailId = orderDetail.OrderDetailId,
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity,
                UnitPrice = orderDetail.UnitPrice,
                ProductName = productName
            };
        }

        public async Task<OrderDetailResponseModel> CreateOrderDetailAsync(OrderDetailRequestModel orderDetailRequest)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = orderDetailRequest.OrderId,
                ProductId = orderDetailRequest.ProductId,
                Quantity = orderDetailRequest.Quantity,
                UnitPrice = orderDetailRequest.UnitPrice
            };

            await _unitOfWork.OrderDetailRepository.AddAsync(orderDetail);
            await _unitOfWork.SaveChangesAsync();

            string? productName = null;
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(orderDetail.ProductId);
            if (product != null)
            {
                productName = product.Name;
            }

            return new OrderDetailResponseModel
            {
                OrderDetailId = orderDetail.OrderDetailId,
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity,
                UnitPrice = orderDetail.UnitPrice,
                ProductName = productName
            };
        }

        public async Task<OrderDetailResponseModel?> UpdateOrderDetailAsync(int id, OrderDetailRequestModel orderDetailRequest)
        {
            var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
            if (orderDetail == null) return null;

            orderDetail.OrderId = orderDetailRequest.OrderId;
            orderDetail.ProductId = orderDetailRequest.ProductId;
            orderDetail.Quantity = orderDetailRequest.Quantity;
            orderDetail.UnitPrice = orderDetailRequest.UnitPrice;

            _unitOfWork.OrderDetailRepository.Update(orderDetail);
            await _unitOfWork.SaveChangesAsync();

            string? productName = null;
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(orderDetail.ProductId);
            if (product != null)
            {
                productName = product.Name;
            }

            return new OrderDetailResponseModel
            {
                OrderDetailId = orderDetail.OrderDetailId,
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity,
                UnitPrice = orderDetail.UnitPrice,
                ProductName = productName
            };
        }

        public async Task<bool> DeleteOrderDetailAsync(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
            if (orderDetail == null) return false;

            _unitOfWork.OrderDetailRepository.Remove(orderDetail);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<OrderDetailResponseModel> OrderDetails, int TotalCount)> GetOrderDetailsAsync(
            string? search = null,
            string? sortBy = null,
            bool ascending = true,
            int page = 1,
            int pageSize = 10,
            string? fields = null,
            int? orderId = null)
        {
            // Get all order details
            var allOrderDetails = (await _unitOfWork.OrderDetailRepository.GetAllAsync()).AsQueryable();
            
            // Include products for product name
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            var productDict = products.ToDictionary(p => p.ProductId, p => p.Name);

            // Apply search filter (search by product name)
            if (!string.IsNullOrEmpty(search))
            {
                var productIds = products.Where(p => p.Name.Contains(search)).Select(p => p.ProductId).ToList();
                allOrderDetails = allOrderDetails.Where(od => productIds.Contains(od.ProductId));
            }

            // Apply order filter
            if (orderId.HasValue)
            {
                allOrderDetails = allOrderDetails.Where(od => od.OrderId == orderId);
            }

            // Get total count before pagination
            var totalCount = allOrderDetails.Count();

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                // Handle special case for ProductName
                if (sortBy.Equals("productName", StringComparison.OrdinalIgnoreCase))
                {
                    var sortDirection = ascending ? "asc" : "desc";
                    allOrderDetails = allOrderDetails.OrderBy(od => productDict.ContainsKey(od.ProductId) ? productDict[od.ProductId] : "").ThenBy(od => od.OrderDetailId);
                    if (!ascending)
                    {
                        allOrderDetails = allOrderDetails.Reverse();
                    }
                }
                else
                {
                    var sortDirection = ascending ? "asc" : "desc";
                    allOrderDetails = allOrderDetails.OrderBy($"{sortBy} {sortDirection}");
                }
            }
            else
            {
                allOrderDetails = allOrderDetails.OrderBy(od => od.OrderDetailId);
            }

            // Apply pagination
            var orderDetails = allOrderDetails.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Map to response models
            var orderDetailResponses = orderDetails.Select(od => new OrderDetailResponseModel
            {
                OrderDetailId = od.OrderDetailId,
                OrderId = od.OrderId,
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
                ProductName = productDict.ContainsKey(od.ProductId) ? productDict[od.ProductId] : null
            });

            return (orderDetailResponses, totalCount);
        }
    }
}