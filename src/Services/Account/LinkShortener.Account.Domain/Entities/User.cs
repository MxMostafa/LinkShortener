using LinkShortener.Account.Domain.Entities.Base;
using LinkShortener.Account.Domain.Enums;


namespace LinkShortener.Account.Domain.Entities;

public class User : ActivateEntity<User, Guid>
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public string? Email { get; private set; }
    public UserStatus UserStatus { get; private set; }
    public bool EnableTwoStep { get; private set; }
    public string PasswordHash { get; private set; } = null!;
    public string PasswordSalt { get; private set; } = null!;
    public int FailedLoginAttempts { get; private set; }
    public DateTime? LockoutEndDate { get; private set; }
    public DateTime? LastLoginDate { get; private set; }
    public string RefreshToken { get; private set; } = null!;
    public long UserProfileId { get; set; }
    public UserProfile UserProfile { get; private set; } = null!;
}
