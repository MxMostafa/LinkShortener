using Microsoft.Extensions.Logging;

namespace LinkShortener.Account.Application.Services.Users;

public class UserLogic : IUserLogic
{
    private readonly ILogger<UserLogic> _logger;

    public UserLogic(ILogger<UserLogic> logger)
    {
        _logger = logger;
    }
}
