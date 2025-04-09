namespace LinkShortener.Account.Application.Common.Errors.Base;

public sealed class Error
{
    public int StatusCode { get; }

    public string Code { get; }

    public string Message { get; }
    public Error(string code, string message, int statusCode)
    {
        Code = code;
        Message = message;
        StatusCode = statusCode;
    }
}
