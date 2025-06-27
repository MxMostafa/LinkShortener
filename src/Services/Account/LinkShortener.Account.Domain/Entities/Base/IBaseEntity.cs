
using LinkShortener.Account.Domain.Entities.Base.Events;

namespace LinkShortener.Account.Domain.Entities.Base;

public interface IBaseEntity
{
    byte[] RowVersion { get; }

    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}
