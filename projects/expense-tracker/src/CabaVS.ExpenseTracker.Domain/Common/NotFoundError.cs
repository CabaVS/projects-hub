namespace CabaVS.ExpenseTracker.Domain.Common;

public sealed record NotFoundError : Error
{
    public NotFoundError(string entity, string message) : base($"{entity}.NotFound", message)
    {
    }
}
