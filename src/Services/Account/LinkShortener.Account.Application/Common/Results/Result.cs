using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LinkShortener.Account.Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && (object)error != null && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && ((object)error == null || error == Error.None))
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
    {
        return new Result(isSuccess: true, Error.Null);
    }

    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new Result<TValue>(value, isSuccess: true, Error.Null);
    }

    public static Result<TValue?> Create<TValue>(TValue? value, Error error) where TValue : class
    {
        if (value == null)
        {
            return Failure<TValue>(error);
        }

        return value;
    }

    public static Result Failure(Error error)
    {
        return new Result(isSuccess: false, error);
    }

    public static Result<TValue?> Failure<TValue>(Error error, TValue? value = default(TValue?))
    {
        return new Result<TValue>(value, isSuccess: false, error);
    }

    public static Result FirstFailureOrSuccess(params Result[] results)
    {
        foreach (Result result in results)
        {
            if (result.IsFailure)
            {
                return result;
            }
        }

        return Success();
    }
}

public class Result<TValue> : Result
{
    public TValue? Value { get; }

    protected internal Result(TValue? value, bool isSuccess, Error? error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    [JsonConstructor]
    public Result(TValue? value, bool isSuccess, bool isFailure, Error? error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static implicit operator Result<TValue>(TValue value)
    {
        return Result.Success(value);
    }
}
