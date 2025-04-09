



namespace LinkShortener.Account.Application.Common.Errors.User;

public static class UserErrors
{
    public static Error UserNameIsEmpty = new("", ValidationMessages.Username_Required, 200);
    public static Error PasswordValidation = new("", ValidationMessages.Password_Format, 200);
}
