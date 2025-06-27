


using LinkShortener.Account.Application.Abstractions.Repositories.Users;

namespace LinkShortener.Account.Application.Services.Users.Commands.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, User>
{
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly IUserWriteRepository _repo;

    public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, IUserWriteRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<Result<User?>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.Create(User.Create(request.UserName, request.Password), cancellationToken);
        return entity;
    }
}
