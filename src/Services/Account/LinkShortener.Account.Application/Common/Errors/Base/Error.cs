namespace LinkShortener.Account.Application.Common.Errors.Base;

public sealed class Error
{
    public int StatusCode { get; }

    public string Code { get; }

    public string Message { get; }


    internal static Error None => new Error(string.Empty, string.Empty, 500);

    internal static Error? Null => null;

    public Error(string code, string message, int statusCode)
    {
        Code = code;
        Message = message;
        StatusCode = statusCode;
    }
}
