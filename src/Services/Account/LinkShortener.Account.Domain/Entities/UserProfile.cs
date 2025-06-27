

using LinkShortener.Account.Domain.Entities.Base;

namespace LinkShortener.Account.Domain.Entities;

public class UserProfile : ActivateEntity<UserProfile,Guid>
{
    public User User { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public long UserId { get; set; }

}
