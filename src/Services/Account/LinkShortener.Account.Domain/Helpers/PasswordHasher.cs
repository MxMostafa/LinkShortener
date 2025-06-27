using LinkShortener.Account.Domain.Models;
using System.Security.Cryptography;
using System.Text;


namespace LinkShortener.Account.Domain.Helpers;

public static class PasswordHasher
{
    public static PasswordHashResult CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        var salt = hmac.Key;
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return new PasswordHashResult(hash, salt);
    }
}
