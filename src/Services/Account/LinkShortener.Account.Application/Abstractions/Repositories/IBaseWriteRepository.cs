

using LinkShortener.Account.Domain.Entities.Base;
using System.Linq.Expressions;

namespace LinkShortener.Account.Application.Abstractions.Repositories;

public interface IBaseWriteRepository<TEntity,in TId> where TEntity : IBaseEntity
{
    Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken);

    Task Update(TEntity entity);

    Task Remove(TEntity entity);
}
