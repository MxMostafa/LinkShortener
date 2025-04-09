
namespace LinkShortener.Account.Api.Common.HttpHandlers;

public interface IMapHttpAuthenticatePolicy
{
    void Apply(IEndpointConventionBuilder endpoint);
}
