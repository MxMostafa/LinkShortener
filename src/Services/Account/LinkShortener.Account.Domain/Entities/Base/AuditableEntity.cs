
using System.ComponentModel.DataAnnotations.Schema;


namespace LinkShortener.Account.Domain.Entities.Base;

public abstract class AuditableEntity<T, TIdType> : Entity<T, TIdType>, IAuditableEntity<TIdType>, IEntity<TIdType>, IBaseEntity where T : AuditableEntity<T, TIdType> where TIdType : IEquatable<TIdType>
{
    public DateTime Created { get; set; }

    public long CreatorId { get; set; }

    public DateTime? Updated { get; set; }

    public long? UpdaterId { get; set; }

    public bool IsDeleted { get; set; }

    [NotMapped]
    public bool CheckUser { get; set; }
}
