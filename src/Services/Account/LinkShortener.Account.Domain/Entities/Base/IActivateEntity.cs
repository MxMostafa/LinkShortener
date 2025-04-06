

namespace LinkShortener.Account.Domain.Entities.Base;

public interface IActivateEntity : IAuditableEntity<long>, IEntity<long>, IBaseEntity
{
    bool IsActive { get; set; }
}
