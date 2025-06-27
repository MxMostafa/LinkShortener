

using LinkShortener.Account.Persistence.Contexts;

namespace LinkShortener.Account.Persistence.Repositories;

public abstract class BaseWriteRepository<TEntity, T> : IBaseWriteRepository<TEntity, T>  where TEntity : class, IEntity<T> where T : IEquatable<T>
{
    protected readonly UserAccountWriteDbContext DbContext;

    protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

    protected BaseWriteRepository(UserAccountWriteDbContext context)
    {
        DbContext = context;
    }


    public virtual async Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken)
    {
        return (await DbSet.AddAsync(entity, cancellationToken)).Entity;
    }

    public virtual Task Update(TEntity entity)
    {
        if (DbContext.Entry(entity).State == EntityState.Unchanged)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task Remove(TEntity entity)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }
}
