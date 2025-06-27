

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
        //await _userLogic.CreateUserr
        return null;
    }
}


