

namespace LinkShortener.Account.Application.Base.Commands;

//semantic abstraction
//contravariant  in TRequest
public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse?>> where TRequest : ICommand<TResponse>
{
}
