namespace CabaVS.ExpenseTracker.Domain.Common;

public static class AsyncResult
{
    public static async Task TapAsync(this Result result, Func<Task> tapAction)
    {
        if (result.IsSuccess)
        {
            await tapAction();
        }
    }
    
    public static async Task<Result<T>> TapAsync<T>(this Result<T> result, Func<T, Task> tapAction)
    {
        if (result.IsSuccess)
        {
            await tapAction(result.Value);
        }

        return result;
    }
    
    public static async Task<Result<TOut>> BindAsync<TOut>(this Result result, Func<Task<Result<TOut>>> bindFunc) =>
        result.IsSuccess
            ? await bindFunc()
            : Result<TOut>.Fail(result.Error);
    
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> bindFunc) =>
        result.IsSuccess
            ? await bindFunc(result.Value)
            : Result<TOut>.Fail(result.Error);
    
    public static async Task<Result<TOut>> MapAsync<TOut>(this Result result, Func<Task<TOut>> mapFunc) =>
        result.IsSuccess
            ? Result<TOut>.Success(await mapFunc())
            : Result<TOut>.Fail(result.Error);
    
    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> mapFunc) =>
        result.IsSuccess
            ? Result<TOut>.Success(await mapFunc(result.Value))
            : Result<TOut>.Fail(result.Error);
}
