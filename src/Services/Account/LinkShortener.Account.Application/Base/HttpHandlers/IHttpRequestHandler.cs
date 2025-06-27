using LinkShortener.Account.Application.Common.Results;

namespace LinkShortener.Account.Application.Base.HttpHandlers;
//semantic abstraction
//contravariant  in TRequest
public interface IHttpRequestHandler<in TRequest> : IRequestHandler<TRequest, Result> where TRequest : IHttpRequest
{
}
