

using LinkShortener.Account.Domain.Entities.Base;
using System.Linq.Expressions;

namespace LinkShortener.Account.Application.Abstractions.Repositories;

public interface IBaseReadRepository<TEntity,in TId> where TEntity : IBaseEntity
{
    ValueTask<TEntity?> FindById(TId id, CancellationToken cancellationToken);

    Task<IList<TEntity>> FindByField(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

}
