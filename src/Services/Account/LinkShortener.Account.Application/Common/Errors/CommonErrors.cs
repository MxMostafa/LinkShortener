

namespace LinkShortener.Account.Application.Common.Errors;

public static class CommonErrors
{
    public static readonly Error ItemNotFound = new Error("NotFound", ValidationMessages. ItemNotFound, 204);

    public static readonly Error ItemIsUsed = new Error("ItemIsUsed", ValidationMessages.ItemIsUsedAndCanNotDeleted, 429);

    public static readonly Error UnknownError = new Error("Provider.Errors", ValidationMessages.ProviderErrors, 502);
    public static Error ValidationError(Dictionary<string, string[]> errors)
    {
        return new Error("Invalid.Arguments", ValidationMessages.Invalid_Arguments + " = " + errors.Keys.FirstOrDefault() + " = " + errors?.Values?.FirstOrDefault(), 400);
    }
}
