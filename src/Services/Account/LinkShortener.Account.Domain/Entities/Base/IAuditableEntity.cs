
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkShortener.Account.Domain.Entities.Base;

public interface IAuditableEntity<T> : IEntity<T>, IBaseEntity
{
    DateTime Created { get; set; }

    long CreatorId { get; set; }

    DateTime? Updated { get; set; }

    long? UpdaterId { get; set; }

    bool IsDeleted { get; set; }

    [NotMapped]
    bool CheckUser { get; set; }
}
