
namespace LinkShortener.Account.Application.Services.Users.Commands.CreateUser;
public record CreateUserCommand(string UserName,string Password):ICommand<User>;
