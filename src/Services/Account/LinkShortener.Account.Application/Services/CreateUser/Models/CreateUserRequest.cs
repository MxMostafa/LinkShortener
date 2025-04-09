
using LinkShortener.Account.Application.HttpHandlers;

namespace LinkShortener.Account.Application.Services.CreateUser.Models;

public record CreateUserRequest(string UserName,string Password): IRequest;
