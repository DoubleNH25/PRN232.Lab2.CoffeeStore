using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN232.Lab2.CoffeeStore.Repositories.Entities;
using PRN232.Lab2.CoffeeStore.Repositories.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Helpers;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.Business;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;
using System.Linq;
using System.Linq.Expressions;

namespace PRN232.Lab2.CoffeeStore.Services.Services;

public class OrderService : IOrderService
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResponseModel<IDictionary<string, object?>>> GetOrdersAsync(PagedRequestModel request)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? DefaultPageSize : Math.Min(request.PageSize, MaxPageSize);

        var query = _unitOfWork.Orders.Query();
        query = query.Include(order => order.OrderDetails);
        query = query.Include(order => order.Payment);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var keyword = request.Search.Trim().ToLowerInvariant();
            query = query.Where(order => order.Status.ToLower().Contains(keyword) || order.UserId.ToLower().Contains(keyword));
        }

        query = ApplySorting(query, request.Sort);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var models = _mapper.Map<List<OrderModel>>(entities);
        var responses = _mapper.Map<List<OrderResponseModel>>(models);
        var selectedItems = FieldSelectionHelper
            .SelectFields(responses.AsEnumerable(), request.Select)
            .ToList();

        return new PagedResponseModel<IDictionary<string, object?>>
        {
            Items = selectedItems,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<IDictionary<string, object?>?> GetOrderByIdAsync(int orderId, string? select)
    {
        var entityQuery = _unitOfWork.Orders.Query();
        entityQuery = entityQuery.Include(order => order.OrderDetails);
        entityQuery = entityQuery.Include(order => order.Payment);
        var entity = await entityQuery.FirstOrDefaultAsync(order => order.OrderId == orderId);
        if (entity == null)
        {
            return null;
        }

        var model = _mapper.Map<OrderModel>(entity);
        var response = _mapper.Map<OrderResponseModel>(model);
        return FieldSelectionHelper.SelectFields(response, select);
    }

    public async Task<OrderResponseModel> CreateOrderAsync(OrderCreateRequestModel request)
    {
        var orderModel = _mapper.Map<OrderModel>(request);
        orderModel.CreatedDate = DateTime.UtcNow;

        var orderEntity = _mapper.Map<Order>(orderModel);
        orderEntity.OrderDetails = new List<OrderDetail>();

        await _unitOfWork.Orders.AddAsync(orderEntity);
        await _unitOfWork.SaveChangesAsync();

        await CreateOrderDetailsAsync(orderEntity.OrderId, request.Details);

        if (request.Payment is not null)
        {
            await UpsertPaymentAsync(orderEntity, request.Payment.Amount, request.Payment.PaymentDate, request.Payment.PaymentMethod);
        }

        await _unitOfWork.SaveChangesAsync();

        var createdQuery = _unitOfWork.Orders.Query();
        createdQuery = createdQuery.Include(order => order.OrderDetails);
        createdQuery = createdQuery.Include(order => order.Payment);
        var createdEntity = await createdQuery.FirstAsync(order => order.OrderId == orderEntity.OrderId);

        var model = _mapper.Map<OrderModel>(createdEntity);
        return _mapper.Map<OrderResponseModel>(model);
    }

    public async Task<OrderResponseModel?> UpdateOrderAsync(int orderId, OrderUpdateRequestModel request)
    {
        var updateQuery = _unitOfWork.Orders.Query();
        updateQuery = updateQuery.Include(order => order.OrderDetails);
        updateQuery = updateQuery.Include(order => order.Payment);
        var orderEntity = await updateQuery.FirstOrDefaultAsync(order => order.OrderId == orderId);
        if (orderEntity == null)
        {
            return null;
        }

        orderEntity.OrderDate = request.OrderDate;
        orderEntity.Status = request.Status;
        orderEntity.UpdatedDate = DateTime.UtcNow;

        await UpdateOrderDetailsAsync(orderEntity, request.Details);

        if (request.Payment is not null)
        {
            await UpsertPaymentAsync(orderEntity, request.Payment.Amount, request.Payment.PaymentDate, request.Payment.PaymentMethod);
        }
        else if (orderEntity.Payment is not null)
        {
            _unitOfWork.Payments.Remove(orderEntity.Payment);
        }

        _unitOfWork.Orders.Update(orderEntity);
        await _unitOfWork.SaveChangesAsync();

        var model = _mapper.Map<OrderModel>(orderEntity);
        return _mapper.Map<OrderResponseModel>(model);
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var orderEntity = await _unitOfWork.Orders.Query()
            .Include(order => order.OrderDetails)
            .Include(order => order.Payment)
            .FirstOrDefaultAsync(order => order.OrderId == orderId);
        if (orderEntity == null)
        {
            return false;
        }

        foreach (var detail in orderEntity.OrderDetails.ToList())
        {
            _unitOfWork.OrderDetails.Remove(detail);
        }

        if (orderEntity.Payment != null)
        {
            _unitOfWork.Payments.Remove(orderEntity.Payment);
        }

        _unitOfWork.Orders.Remove(orderEntity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private async Task CreateOrderDetailsAsync(int orderId, List<OrderDetailCreateRequestModel> details)
    {
        foreach (var detailRequest in details)
        {
            var productEntity = await _unitOfWork.Products.GetByIdAsync(detailRequest.ProductId);
            if (productEntity == null)
            {
                throw new InvalidOperationException($"Product with id {detailRequest.ProductId} not found.");
            }

            var newDetail = new OrderDetail
            {
                OrderId = orderId,
                ProductId = detailRequest.ProductId,
                Quantity = detailRequest.Quantity,
                UnitPrice = detailRequest.UnitPrice,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.OrderDetails.AddAsync(newDetail);
        }
    }

    private async Task UpdateOrderDetailsAsync(Order orderEntity, List<OrderDetailUpdateRequestModel> details)
    {
        var existingDetails = orderEntity.OrderDetails.ToDictionary(detail => detail.OrderDetailId);
        var retainedDetailIds = new HashSet<int>();

        foreach (var detailRequest in details)
        {
            if (detailRequest.OrderDetailId == 0)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(detailRequest.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with id {detailRequest.ProductId} not found.");
                }

                var newDetail = new OrderDetail
                {
                    OrderId = orderEntity.OrderId,
                    ProductId = detailRequest.ProductId,
                    Quantity = detailRequest.Quantity,
                    UnitPrice = detailRequest.UnitPrice,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.OrderDetails.AddAsync(newDetail);
                continue;
            }

            if (!existingDetails.TryGetValue(detailRequest.OrderDetailId, out var existingDetail))
            {
                continue;
            }

            var existingProduct = await _unitOfWork.Products.GetByIdAsync(detailRequest.ProductId);
            if (existingProduct == null)
            {
                throw new InvalidOperationException($"Product with id {detailRequest.ProductId} not found.");
            }

            existingDetail.ProductId = detailRequest.ProductId;
            existingDetail.Quantity = detailRequest.Quantity;
            existingDetail.UnitPrice = detailRequest.UnitPrice;
            existingDetail.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.OrderDetails.Update(existingDetail);
            retainedDetailIds.Add(existingDetail.OrderDetailId);
        }

        foreach (var existingDetail in existingDetails.Values)
        {
            if (!retainedDetailIds.Contains(existingDetail.OrderDetailId))
            {
                _unitOfWork.OrderDetails.Remove(existingDetail);
            }
        }
    }

    private async Task UpsertPaymentAsync(Order orderEntity, decimal amount, DateTime paymentDate, string paymentMethod)
    {
        if (orderEntity.Payment == null)
        {
            var payment = new Payment
            {
                OrderId = orderEntity.OrderId,
                Amount = amount,
                PaymentDate = paymentDate,
                PaymentMethod = paymentMethod,
                CreatedDate = DateTime.UtcNow
            };
            await _unitOfWork.Payments.AddAsync(payment);
        }
        else
        {
            orderEntity.Payment.Amount = amount;
            orderEntity.Payment.PaymentDate = paymentDate;
            orderEntity.Payment.PaymentMethod = paymentMethod;
            orderEntity.Payment.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.Payments.Update(orderEntity.Payment);
        }
    }

    private static IQueryable<Order> ApplySorting(IQueryable<Order> query, string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return query.OrderBy(order => order.OrderId);
        }

        var sortFields = sort
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToArray();

        IOrderedQueryable<Order>? orderedQuery = null;

        foreach (var field in sortFields)
        {
            var descending = field.StartsWith("-", StringComparison.Ordinal);
            var fieldName = descending ? field[1..] : field;

            Expression<Func<Order, object>>? selector = fieldName.ToLowerInvariant() switch
            {
                "orderdate" => order => order.OrderDate,
                "status" => order => order.Status,
                "createddate" => order => order.CreatedDate,
                "updateddate" => order => order.UpdatedDate ?? DateTime.MinValue,
                _ => null
            };

            if (selector == null)
            {
                continue;
            }

            if (orderedQuery == null)
            {
                orderedQuery = descending
                    ? query.OrderByDescending(selector)
                    : query.OrderBy(selector);
            }
            else
            {
                orderedQuery = descending
                    ? orderedQuery.ThenByDescending(selector)
                    : orderedQuery.ThenBy(selector);
            }
        }

        return orderedQuery ?? query.OrderBy(order => order.OrderId);
    }
}
