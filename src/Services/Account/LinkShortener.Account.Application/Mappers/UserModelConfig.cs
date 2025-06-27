


using LinkShortener.Account.Application.Services.Users.Commands.CreateUser;
using LinkShortener.Account.Application.Services.Users.Models.CreateUser;

namespace LinkShortener.Account.Application.Mappers;

public class UserModelConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserRequest, CreateUserCommand>();

        config.NewConfig<User, CreateUserResponse>()
            .Map(d => d.UserId, s => s.Id);
    }
}
