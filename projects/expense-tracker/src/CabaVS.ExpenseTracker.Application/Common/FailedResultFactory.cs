using CabaVS.ExpenseTracker.Domain.Common;

namespace CabaVS.ExpenseTracker.Application.Common;

internal static class FailedResultFactory
{
    public static TResponse Create<TResponse>(Error error) where TResponse : Result
    {
        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)Result.Fail(error);
        }

        var result = typeof(Result<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResponse).GetGenericArguments()[0])
            .GetMethod(nameof(Result.Fail))!
            .Invoke(null, [error])!;
        return (TResponse)result;
    }
}
