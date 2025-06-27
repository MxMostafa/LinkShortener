using LinkShortener.Account.Application.Common.Results;
using LinkShortener.Account.Application.Services.Users.Models.CreateUser;

namespace LinkShortener.Account.Application.Services.Users;

public interface IUserLogic
{
    Task<Result<CreateUserResponse?>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
}
