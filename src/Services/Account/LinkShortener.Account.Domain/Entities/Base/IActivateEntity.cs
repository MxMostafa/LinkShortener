

namespace LinkShortener.Account.Domain.Entities.Base;

public interface IActivateEntity<T> : IAuditableEntity<T>, IEntity<T>, IBaseEntity
{
    bool IsActive { get; set; }
}
