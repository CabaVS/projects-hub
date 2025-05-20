namespace CabaVS.ExpenseTracker.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private readonly Error _error;
    public Error Error => IsFailure
        ? _error
        : throw new InvalidOperationException($"Unable to access '{nameof(Error)}' property on successful Result.");
    
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid parameters passed into Result's constructor.", nameof(error));
        }
        
        IsSuccess = isSuccess;
        _error = error;
    }
    
    public static Result Success() => new(true, Error.None);
    public static Result Fail(Error error) => new(false, error);
    
    public static implicit operator Result(Error error) => Fail(error);
}

public class Result<T> : Result
{
    private readonly T? _value;
    public T Value => IsSuccess 
        ? _value!
        : throw new InvalidOperationException($"Unable to access '{nameof(Value)}' property on a failed Result.");
    
    protected Result(bool isSuccess, Error error, T? value) : base(isSuccess, error) => _value = value;
    
    public static Result<T> Success(T value) => new(true, Error.None, value);
    public new static Result<T> Fail(Error error) => new(false, error, default);
    
    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Fail(error);
}
