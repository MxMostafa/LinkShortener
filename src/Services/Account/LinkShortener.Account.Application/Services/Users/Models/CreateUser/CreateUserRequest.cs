using LinkShortener.Account.Application.Base.HttpHandlers;

namespace LinkShortener.Account.Application.Services.Users.Models.CreateUser;

public record CreateUserRequest(string UserName,string Password): IHttpRequest;
