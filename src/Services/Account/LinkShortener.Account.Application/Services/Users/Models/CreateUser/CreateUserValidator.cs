﻿namespace LinkShortener.Account.Application.Services.Users.Models.CreateUser;
public class CreateUserValidator: AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(_ => _.UserName)
            .NotEmpty().WithError(UserErrors.UserNameIsEmpty);

        RuleFor(_ => _.Password)
           .Matches(RegexPattern.Password)!.WithError(UserErrors.PasswordValidation);
    }
}
