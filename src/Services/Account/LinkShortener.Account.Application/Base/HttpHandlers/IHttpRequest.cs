using LinkShortener.Account.Application.Common.Results;

namespace LinkShortener.Account.Application.Base.HttpHandlers;

//semantic abstraction
public interface IHttpRequest : IRequest<Result>, IBaseRequest //Explicitness
{
}
