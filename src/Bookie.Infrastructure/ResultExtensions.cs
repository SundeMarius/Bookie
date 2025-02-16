using System;
using FluentMonads;

namespace Bookie.Infrastructure;

public static class ResultExtensions
{
    public static Task<Result<TNew>> AndThenAsync<T, TNew>(this Result<T> result, Func<T, Task<Result<TNew>>> func)
    {
        if (!result.IsSuccess)
        {
            return Task.FromResult(Result<TNew>.Failure(result.Error));
        }

        return func(result.Value);
    }
}
