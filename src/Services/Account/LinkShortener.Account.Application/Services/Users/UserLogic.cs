



namespace LinkShortener.Account.Application.Services.Users;

public class UserLogic : IUserLogic
{
    private readonly ILogger<UserLogic> _logger;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    public UserLogic(ILogger<UserLogic> logger, IMediator mediator, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateUserResponse?>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var isValidRequest = await request.IsValidAsync<CreateUserValidator, CreateUserRequest>(cancellationToken);
        if (isValidRequest.IsFailure)
            return Result.Failure<CreateUserResponse?>(isValidRequest.Error!);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        var response = await _mediator.Send(request.Adapt<CreateUserCommand>(), cancellationToken);

        if (response.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<CreateUserResponse>(response.Error!);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return response.Value!.Adapt<CreateUserResponse>();

    }
}
