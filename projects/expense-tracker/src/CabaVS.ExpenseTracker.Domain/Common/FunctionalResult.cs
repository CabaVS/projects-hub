namespace CabaVS.ExpenseTracker.Domain.Common;

public static class FunctionalResult
{
    public static Result Tap(this Result result, Action tapAction)
    {
        if (result.IsSuccess)
        {
            tapAction();
        }
        
        return result;
    }
    
    public static Result<T> Tap<T>(this Result<T> result, Action<T> tapAction)
    {
        if (result.IsSuccess)
        {
            tapAction(result.Value);
        }
        
        return result;
    }
    
    public static Result<TOut> Bind<TOut>(this Result result, Func<Result<TOut>> bindFunc) =>
        result.IsSuccess
            ? bindFunc()
            : Result<TOut>.Fail(result.Error);
    
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> bindFunc) =>
        result.IsSuccess
            ? bindFunc(result.Value)
            : Result<TOut>.Fail(result.Error);
    
    public static Result<TOut> Map<TOut>(this Result result, Func<TOut> mapFunc) =>
        result.IsSuccess
            ? Result<TOut>.Success(mapFunc())
            : Result<TOut>.Fail(result.Error);
    
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapFunc) =>
        result.IsSuccess
            ? Result<TOut>.Success(mapFunc(result.Value))
            : Result<TOut>.Fail(result.Error);
    
    public static TOut Match<TOut>(this Result result, Func<TOut> successFunc, Func<Error, TOut> failFunc) =>
        result.IsSuccess
            ? successFunc()
            : failFunc(result.Error);
    
    public static TOut Match<TResult, TOut>(this Result<TResult> result, Func<TResult, TOut> successFunc, Func<Error, TOut> failFunc) =>
        result.IsSuccess
            ? successFunc(result.Value)
            : failFunc(result.Error);
    
    public static Result Ensure(this Result result, Func<bool> condition, Error error) =>
        result.IsFailure
            ? result
            : condition()
                ? result
                : Result.Fail(error);
    
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> condition, Error error) =>
        result.IsFailure
            ? result
            : condition(result.Value)
                ? result
                : Result<T>.Fail(error);

    public static Result<string> EnsureStringNotNullOrEmpty(this Result<string> result, Error error) =>
        result.Ensure(
            x => !string.IsNullOrEmpty(x),
            error);

    public static Result<string> EnsureStringNotNullOrWhitespace(this Result<string> result, Error error) =>
        result.Ensure(
            x => !string.IsNullOrWhiteSpace(x),
            error);

    public static Result<string> EnsureStringNotTooLong(this Result<string> result, int maxLength, Error error) =>
        result.Ensure(
            x => x.Length <= maxLength,
            error);

    public static Result<string> EnsureStringNotTooShort(this Result<string> result, int minLength, Error error) =>
        result.Ensure(
            x => x.Length >= minLength,
            error);
}
