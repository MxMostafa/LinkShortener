

using LinkShortener.Account.Application.Common.Results;

namespace LinkShortener.Account.Application.Base.Queries;
//semantic abstraction
public class IQuery<TResponse> : IRequest<Result<TResponse?>>, IBaseRequest //Explicitness
{
}
