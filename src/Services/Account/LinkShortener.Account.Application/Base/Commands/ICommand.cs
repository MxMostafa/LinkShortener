

using LinkShortener.Account.Application.Common.Results;

namespace LinkShortener.Account.Application.Base.Commands;
//semantic abstraction
public interface ICommand<TResponse>:IRequest<Result<TResponse?>>, IBaseRequest //Explicitness
{
}
