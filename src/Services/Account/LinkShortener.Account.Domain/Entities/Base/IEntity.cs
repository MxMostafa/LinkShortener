

namespace LinkShortener.Account.Domain.Entities.Base;

public interface IEntity<out T> : IBaseEntity
{
    T Id { get; }
}
