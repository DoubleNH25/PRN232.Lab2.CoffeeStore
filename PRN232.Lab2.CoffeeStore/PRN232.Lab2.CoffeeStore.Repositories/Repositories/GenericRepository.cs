using Microsoft.EntityFrameworkCore;
using PRN232.Lab2.CoffeeStore.Repositories.Entities;
using PRN232.Lab2.CoffeeStore.Repositories.Interfaces;

namespace PRN232.Lab2.CoffeeStore.Repositories.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
{
    protected readonly DbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public GenericRepository(DbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public IQueryable<TEntity> Query()
    {
        IQueryable<TEntity> query = DbSet.AsQueryable();

        if (typeof(BaseEntity).IsAssignableFrom(typeof(TEntity)))
        {
            query = query.Where(entity => !EF.Property<bool>(entity, nameof(BaseEntity.IsDeleted)));
        }

        return query;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>>? predicate = null, params System.Linq.Expressions.Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = Query();

        if (includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(object id)
    {
        var entity = await DbSet.FindAsync(id);

        if (entity is BaseEntity baseEntity && baseEntity.IsDeleted)
        {
            return null;
        }

        return entity;
    }

    public async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.IsDeleted = true;
            baseEntity.UpdatedDate = DateTime.UtcNow;
            DbSet.Update(entity);
            return;
        }

        DbSet.Remove(entity);
    }
}
