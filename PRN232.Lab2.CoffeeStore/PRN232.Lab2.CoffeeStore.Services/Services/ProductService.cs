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

public class ProductService : IProductService
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResponseModel<IDictionary<string, object?>>> GetProductsAsync(PagedRequestModel request)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? DefaultPageSize : Math.Min(request.PageSize, MaxPageSize);

        var query = _unitOfWork.Products.Query();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var keyword = request.Search.Trim().ToLowerInvariant();
            query = query.Where(product =>
                product.Name.ToLower().Contains(keyword) ||
                (product.Description != null && product.Description.ToLower().Contains(keyword)));
        }

        query = ApplySorting(query, request.Sort);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var models = _mapper.Map<List<ProductModel>>(entities);
        var responses = _mapper.Map<List<ProductResponseModel>>(models);
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

    public async Task<IDictionary<string, object?>?> GetProductByIdAsync(int productId, string? select)
    {
        var entity = await _unitOfWork.Products.GetByIdAsync(productId);
        if (entity == null)
        {
            return null;
        }

        var model = _mapper.Map<ProductModel>(entity);
        var response = _mapper.Map<ProductResponseModel>(model);
        return FieldSelectionHelper.SelectFields(response, select);
    }

    public async Task<ProductResponseModel> CreateProductAsync(ProductCreateRequestModel request)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
        if (category == null)
        {
            throw new InvalidOperationException($"Category with id {request.CategoryId} not found.");
        }

        var model = _mapper.Map<ProductModel>(request);
        model.CreatedDate = DateTime.UtcNow;
        var entity = _mapper.Map<Product>(model);

        await _unitOfWork.Products.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        var createdModel = _mapper.Map<ProductModel>(entity);
        return _mapper.Map<ProductResponseModel>(createdModel);
    }

    public async Task<ProductResponseModel?> UpdateProductAsync(int productId, ProductUpdateRequestModel request)
    {
        var entity = await _unitOfWork.Products.GetByIdAsync(productId);
        if (entity == null)
        {
            return null;
        }

        var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
        if (category == null)
        {
            throw new InvalidOperationException($"Category with id {request.CategoryId} not found.");
        }

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;
        entity.IsActive = request.IsActive;
        entity.CategoryId = request.CategoryId;
        entity.UpdatedDate = DateTime.UtcNow;

        _unitOfWork.Products.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        var model = _mapper.Map<ProductModel>(entity);
        return _mapper.Map<ProductResponseModel>(model);
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var entity = await _unitOfWork.Products.GetByIdAsync(productId);
        if (entity == null)
        {
            return false;
        }

        _unitOfWork.Products.Remove(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static IQueryable<Product> ApplySorting(IQueryable<Product> query, string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return query.OrderBy(product => product.ProductId);
        }

        var sortFields = sort
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToArray();

        IOrderedQueryable<Product>? orderedQuery = null;

        foreach (var field in sortFields)
        {
            var descending = field.StartsWith("-", StringComparison.Ordinal);
            var fieldName = descending ? field[1..] : field;

            Expression<Func<Product, object>>? selector = fieldName.ToLowerInvariant() switch
            {
                "name" => product => product.Name,
                "price" => product => product.Price,
                "isactive" => product => product.IsActive,
                "createddate" => product => product.CreatedDate,
                "updateddate" => product => product.UpdatedDate ?? DateTime.MinValue,
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

        return orderedQuery ?? query.OrderBy(product => product.ProductId);
    }
}
