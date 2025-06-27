using LinkShortener.Account.Application.Base.HttpHandlers;

namespace LinkShortener.Account.Application.Services.Users.CreateUser.Models;

public record CreateUserRequest(string UserName,string Password): IHttpRequest;
