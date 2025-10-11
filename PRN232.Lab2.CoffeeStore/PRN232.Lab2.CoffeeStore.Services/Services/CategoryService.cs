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

public class CategoryService : ICategoryService
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResponseModel<IDictionary<string, object?>>> GetCategoriesAsync(PagedRequestModel request)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? DefaultPageSize : Math.Min(request.PageSize, MaxPageSize);

        var query = _unitOfWork.Categories.Query();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var keyword = request.Search.Trim().ToLowerInvariant();
            query = query.Where(category =>
                category.Name.ToLower().Contains(keyword) ||
                (category.Description != null && category.Description.ToLower().Contains(keyword)));
        }

        query = ApplySorting(query, request.Sort);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var models = _mapper.Map<List<CategoryModel>>(entities);
        var responses = _mapper.Map<List<CategoryResponseModel>>(models);
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

    public async Task<IDictionary<string, object?>?> GetCategoryByIdAsync(int categoryId, string? select)
    {
        var entity = await _unitOfWork.Categories.GetByIdAsync(categoryId);
        if (entity == null)
        {
            return null;
        }

        var model = _mapper.Map<CategoryModel>(entity);
        var response = _mapper.Map<CategoryResponseModel>(model);
        return FieldSelectionHelper.SelectFields(response, select);
    }

    public async Task<CategoryResponseModel> CreateCategoryAsync(CategoryCreateRequestModel request)
    {
        var model = _mapper.Map<CategoryModel>(request);
        model.CreatedDate = DateTime.UtcNow;
        var entity = _mapper.Map<Category>(model);

        await _unitOfWork.Categories.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        var createdModel = _mapper.Map<CategoryModel>(entity);
        return _mapper.Map<CategoryResponseModel>(createdModel);
    }

    public async Task<CategoryResponseModel?> UpdateCategoryAsync(int categoryId, CategoryUpdateRequestModel request)
    {
        var entity = await _unitOfWork.Categories.GetByIdAsync(categoryId);
        if (entity == null)
        {
            return null;
        }

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.UpdatedDate = DateTime.UtcNow;

        _unitOfWork.Categories.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        var model = _mapper.Map<CategoryModel>(entity);
        return _mapper.Map<CategoryResponseModel>(model);
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        var entity = await _unitOfWork.Categories.GetByIdAsync(categoryId);
        if (entity == null)
        {
            return false;
        }

        _unitOfWork.Categories.Remove(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static IQueryable<Category> ApplySorting(IQueryable<Category> query, string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return query.OrderBy(category => category.CategoryId);
        }

        var sortFields = sort
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToArray();

        IOrderedQueryable<Category>? orderedQuery = null;

        foreach (var field in sortFields)
        {
            var descending = field.StartsWith("-", StringComparison.Ordinal);
            var fieldName = descending ? field[1..] : field;

            Expression<Func<Category, object>>? selector = fieldName.ToLowerInvariant() switch
            {
                "name" => category => category.Name,
                "createddate" => category => category.CreatedDate,
                "updateddate" => category => category.UpdatedDate ?? DateTime.MinValue,
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

        return orderedQuery ?? query.OrderBy(category => category.CategoryId);
    }
}
