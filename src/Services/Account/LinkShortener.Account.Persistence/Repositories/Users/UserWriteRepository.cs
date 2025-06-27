

using LinkShortener.Account.Application.Abstractions.Repositories.Users;
using LinkShortener.Account.Persistence.Contexts;

namespace LinkShortener.Account.Persistence.Repositories.Users;

public class UserWriteRepository : BaseWriteRepository<User, Guid>, IUserWriteRepository
{
    public UserWriteRepository(UserAccountWriteDbContext context) : base(context)
    {
    }
}
