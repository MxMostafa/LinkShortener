

namespace LinkShortener.Account.Domain.Entities.Base;

public abstract class ActivateEntity<T,TIdType> : AuditableEntity<T, TIdType>, IActivateEntity<TIdType>, IAuditableEntity<TIdType>, IEntity<TIdType>, IBaseEntity where T : ActivateEntity<T, TIdType> where TIdType : IEquatable<TIdType>
{
    public bool IsActive { get; set; }

    protected ActivateEntity()
    {
    }
}
