

using LinkShortener.Account.Application.Abstractions.Repositories.Users;
using LinkShortener.Account.Persistence.Contexts;

namespace LinkShortener.Account.Persistence.Repositories.Users;

public class UserReadRepository : BaseReadRepository<User, Guid>, IUserReadRepository
{
    public UserReadRepository(UserAccountReadDbContext context) : base(context)
    {
    }
}
