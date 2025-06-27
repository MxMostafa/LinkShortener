

using LinkShortener.Account.Application.Services.Users.Models.CreateUser;

namespace LinkShortener.Account.Api.Endpoints.Users.Commands;

public class CreateUserRequestHandler : IHttpRequestHandler<CreateUserRequest>
{
    private readonly IUserLogic _userLogic;

    public CreateUserRequestHandler(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }

    public async Task<Result> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
       var response= await _userLogic.CreateUserAsync(request,cancellationToken);
        return response;
    }
}


