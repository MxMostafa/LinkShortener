

namespace LinkShortener.Account.Domain.Models;

public record PasswordHashResult(byte[] Hash, byte[] Salt);
