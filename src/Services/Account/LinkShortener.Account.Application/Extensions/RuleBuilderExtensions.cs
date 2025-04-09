


using LinkShortener.Account.Application.Common.Errors.Base;

namespace LinkShortener.Account.Application.Extensions;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        rule.WithMessage(error.Message).WithErrorCode(error.Code);
        return rule;
    }
}
