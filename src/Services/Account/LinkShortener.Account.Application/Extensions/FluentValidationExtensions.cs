





namespace LinkShortener.Account.Application.Extensions;

public static class FluentValidationExtensions
{
    public static async Task<Result<bool>> IsValidAsync<TValidator, T>(this T request, CancellationToken cancellationToken) where TValidator : AbstractValidator<T> where T : class
    {
        ValidationResult validationResult = await Activator.CreateInstance<TValidator>().ValidateAsync(request, cancellationToken);
        return validationResult.IsValid ? ((Result<bool>)true) : Result.Failure(MapValidationFailureToError(validationResult.Errors), value: false);
    }

    private static Error MapValidationFailureToError(IEnumerable<ValidationFailure> validationFailures)
    {
        return CommonErrors.ValidationError((from e in validationFailures
                                             group e by e.PropertyName).ToDictionary((IGrouping<string, ValidationFailure> v) => v.Key, (IGrouping<string, ValidationFailure> v) => v.Select((ValidationFailure x) => x.ErrorMessage).ToArray()));
    }
}
