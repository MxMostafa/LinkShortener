

using LinkShortener.Account.Persistence.Contexts;

namespace LinkShortener.Account.Persistence.Repositories;

public abstract class BaseReadRepository<TEntity, T> : IBaseReadRepository<TEntity, T>  where TEntity : class, IEntity<T> where T : IEquatable<T>
{
    protected readonly UserAccountReadDbContext DbContext;

    protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

    protected BaseReadRepository(UserAccountReadDbContext context)
    {
        DbContext = context;
    }

    public virtual ValueTask<TEntity?> FindById(T id, CancellationToken cancellationToken)
    {
        return DbSet.FindWithNoLockAsync(new object[1] { id }, cancellationToken);
    }

    public async Task<IList<TEntity>> FindByField(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await EntityFrameworkQueryableExtensions.AsNoTracking(DbSet).Where(predicate).ToListWithNoLockAsync(cancellationToken);
    }
}
