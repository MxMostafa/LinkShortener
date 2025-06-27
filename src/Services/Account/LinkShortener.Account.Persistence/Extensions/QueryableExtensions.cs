
using System.Transactions;

namespace LinkShortener.Account.Persistence.Extensions;

public static class QueryableExtensions
{
    public static async ValueTask<T?> FindWithNoLockAsync<T>(this DbSet<T> dbSet, object?[]? keyValues, CancellationToken cancellationToken) where T : class
    {
        using TransactionScope scope = CreateTransactionAsync();
        T? result = await dbSet.FindAsync(keyValues, cancellationToken);
        scope.Complete();
        return result;
    }


    public static async Task<List<T>> ToListWithNoLockAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default(CancellationToken), Expression<Func<T, bool>>? expression = null)
    {
        using TransactionScope scope = CreateTransactionAsync();
        if (expression != null)
        {
            query = query.Where(expression);
        }

        List<T> result = await EntityFrameworkQueryableExtensions.ToListAsync(query, cancellationToken);
        scope.Complete();
        return result;
    }

    private static TransactionScope CreateTransactionAsync()
    {
        return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadUncommitted
        }, TransactionScopeAsyncFlowOption.Enabled);
    }
}
