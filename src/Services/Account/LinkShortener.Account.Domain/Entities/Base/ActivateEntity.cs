

namespace LinkShortener.Account.Domain.Entities.Base;

public abstract class ActivateEntity<T> : AuditableEntity<T, long>, IActivateEntity, IAuditableEntity<long>, IEntity<long>, IBaseEntity where T : ActivateEntity<T>
{
    public bool IsActive { get; set; }

    protected ActivateEntity()
    {
    }
}
