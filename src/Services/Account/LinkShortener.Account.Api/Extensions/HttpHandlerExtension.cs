using LinkShortener.Account.Api.Common.HttpHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace LinkShortener.Account.Api.Extensions;

public static class HttpHandlerExtension
{
    public static IEndpointRouteBuilder MapHttpPost<TRequest, TResponse>(this IEndpointRouteBuilder endpoints, string template,Func< MapHttpConfiguration> ? config=null)
        where TRequest : IHttpRequest
    {
        var endpoint=endpoints.MapPost(template,
             async(IMediator mediator, [FromBody] TRequest request, CancellationToken cancellationToken)=>
              await mediator.Send(request, cancellationToken)).Produces<TResponse>();

        if (config is not null)
        {
            SetConfiguration(endpoint, config.Invoke());
        }

        return endpoints;
    }

    private static void SetConfiguration(IEndpointConventionBuilder endpoint, MapHttpConfiguration configuration)
    {
        if (configuration.AllowAnonymous)
        {
            endpoint.AllowAnonymous();
        }
        else
        {
            if (configuration.Policy is null)
            {
                endpoint.RequireAuthorization();
            }
            else
            {
                configuration.Policy.Apply(endpoint);
            }
        }

        if (configuration.Description is not null)
        {
            endpoint.WithDescription(configuration.Description);
        }

        if (configuration.Name is not null)
        {
            endpoint.WithName(configuration.Name);
        }

        if (configuration.Summary is not null)
        {
            endpoint.WithSummary(configuration.Summary);
        }

        endpoint.WithOpenApi();
    }
}
