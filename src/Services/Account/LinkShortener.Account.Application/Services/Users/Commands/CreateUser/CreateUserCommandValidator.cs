
namespace LinkShortener.Account.Application.Services.Users.Commands.CreateUser;

public class CreateUserCommandValidator:AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(_ => _.UserName)
         .NotEmpty().WithError(UserErrors.UserNameIsEmpty);

        RuleFor(_ => _.Password)
           .Matches(RegexPattern.Password)!.WithError(UserErrors.PasswordValidation);
    }
}
