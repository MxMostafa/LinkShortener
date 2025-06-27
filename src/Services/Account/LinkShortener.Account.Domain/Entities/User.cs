

using LinkShortener.Account.Domain.Helpers;

namespace LinkShortener.Account.Domain.Entities;

public class User : ActivateEntity<User, Guid>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public User()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {

    }

    public string UserName { get; private set; }
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

    public static User Create(string userName, string password)
    {
        Guard.Against.NullOrWhiteSpace(userName, nameof(userName));
        Guard.Against.NullOrWhiteSpace(password, nameof(password));

        var user = new User();
        user.UserName = userName;

        var passwordResult = PasswordHasher.CreatePasswordHash(password);
        user.PasswordHash = Convert.ToBase64String(passwordResult.Hash);
        user.PasswordSalt = Convert.ToBase64String(passwordResult.Salt);

        return user;
    }
}
