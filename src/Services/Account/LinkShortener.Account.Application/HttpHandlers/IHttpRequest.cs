﻿
using LinkShortener.Account.Application.Common.Results;

namespace LinkShortener.Account.Application.HttpHandlers;
public interface IHttpRequest : IRequest<Result>, IBaseRequest
{
}
