

using LinkShortener.Account.Application.Common.Results;

namespace LinkShortener.Account.Application.HttpHandlers;

public interface IHttpRequestHandler<in TRequest> : IRequestHandler<TRequest, Result> where TRequest : IHttpRequest
{
}
