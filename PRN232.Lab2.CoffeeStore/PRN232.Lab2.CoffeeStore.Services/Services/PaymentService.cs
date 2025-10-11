using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN232.Lab2.CoffeeStore.Repositories.Entities;
using PRN232.Lab2.CoffeeStore.Repositories.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Helpers;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.Business;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;
using System.Linq.Expressions;

namespace PRN232.Lab2.CoffeeStore.Services.Services;

public class PaymentService : IPaymentService
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResponseModel<IDictionary<string, object?>>> GetPaymentsAsync(PagedRequestModel request)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? DefaultPageSize : Math.Min(request.PageSize, MaxPageSize);

        var query = _unitOfWork.Payments.Query();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var keyword = request.Search.Trim().ToLowerInvariant();
            query = query.Where(payment =>
                payment.PaymentMethod.ToLower().Contains(keyword));
        }

        query = ApplySorting(query, request.Sort);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var models = _mapper.Map<List<PaymentModel>>(entities);
        var responses = _mapper.Map<List<PaymentResponseModel>>(models);
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

    public async Task<IDictionary<string, object?>?> GetPaymentByIdAsync(int paymentId, string? select)
    {
        var entity = await _unitOfWork.Payments.GetByIdAsync(paymentId);
        if (entity == null)
        {
            return null;
        }

        var model = _mapper.Map<PaymentModel>(entity);
        var response = _mapper.Map<PaymentResponseModel>(model);
        return FieldSelectionHelper.SelectFields(response, select);
    }

    public async Task<PaymentResponseModel> CreatePaymentAsync(int orderId, PaymentCreateRequestModel request)
    {
        var order = await _unitOfWork.Orders.Query()
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
        if (order == null)
        {
            throw new InvalidOperationException($"Order with id {orderId} not found.");
        }

        if (order.Payment != null)
        {
            throw new InvalidOperationException($"Order {orderId} already has a payment record.");
        }

        var model = _mapper.Map<PaymentModel>(request);
        model.OrderId = orderId;
        model.CreatedDate = DateTime.UtcNow;

        var entity = _mapper.Map<Payment>(model);
        await _unitOfWork.Payments.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        var createdModel = _mapper.Map<PaymentModel>(entity);
        return _mapper.Map<PaymentResponseModel>(createdModel);
    }

    public async Task<PaymentResponseModel?> UpdatePaymentAsync(int paymentId, PaymentUpdateRequestModel request)
    {
        var entity = await _unitOfWork.Payments.GetByIdAsync(paymentId);
        if (entity == null)
        {
            return null;
        }

        entity.Amount = request.Amount;
        entity.PaymentDate = request.PaymentDate;
        entity.PaymentMethod = request.PaymentMethod;
        entity.UpdatedDate = DateTime.UtcNow;

        _unitOfWork.Payments.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        var model = _mapper.Map<PaymentModel>(entity);
        return _mapper.Map<PaymentResponseModel>(model);
    }

    public async Task<bool> DeletePaymentAsync(int paymentId)
    {
        var entity = await _unitOfWork.Payments.GetByIdAsync(paymentId);
        if (entity == null)
        {
            return false;
        }

        _unitOfWork.Payments.Remove(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static IQueryable<Payment> ApplySorting(IQueryable<Payment> query, string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return query.OrderBy(payment => payment.PaymentId);
        }

        var sortFields = sort
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToArray();

        IOrderedQueryable<Payment>? orderedQuery = null;

        foreach (var field in sortFields)
        {
            var descending = field.StartsWith("-", StringComparison.Ordinal);
            var fieldName = descending ? field[1..] : field;

            Expression<Func<Payment, object>>? selector = fieldName.ToLowerInvariant() switch
            {
                "amount" => payment => payment.Amount,
                "paymentdate" => payment => payment.PaymentDate,
                "paymentmethod" => payment => payment.PaymentMethod,
                "createddate" => payment => payment.CreatedDate,
                "updateddate" => payment => payment.UpdatedDate ?? DateTime.MinValue,
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

        return orderedQuery ?? query.OrderBy(payment => payment.PaymentId);
    }
}
