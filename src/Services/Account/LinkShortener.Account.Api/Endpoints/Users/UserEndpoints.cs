

using LinkShortener.Account.Api.Common.HttpHandlers;
using LinkShortener.Account.Api.Extensions;
using LinkShortener.Account.Application.Common.Results;
using LinkShortener.Account.Application.Services.Users.Models.CreateUser;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Account.Api.Endpoints.Users;

public class UserEndpoints : ICarterModule
{
    private const string ActionEndpointRoute = "/api/Account/v1/User";
    private const string ActionEndpointTag = "User";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
 
        var group = app
          .MapGroup(ActionEndpointRoute)
          .WithTags(ActionEndpointTag);

        group.MapHttpPost<CreateUserRequest, Result<CreateUserResponse>>("/users",
          () => new MapHttpConfiguration
          {
              AllowAnonymous = true,
          });
    }
}
