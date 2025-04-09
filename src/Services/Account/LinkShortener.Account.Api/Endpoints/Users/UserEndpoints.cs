

using LinkShortener.Account.Application.Services.CreateUser.Models;

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

        group.MapPost("/users", async (IMediator mediator, [FromBody] CreateUserRequest request, CancellationToken cancellationToken) =>
        {
            return await mediator.Send(request, cancellationToken);
        });

    }
}
