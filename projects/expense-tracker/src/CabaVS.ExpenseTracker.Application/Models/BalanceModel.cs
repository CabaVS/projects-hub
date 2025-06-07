namespace CabaVS.ExpenseTracker.Application.Models;

public sealed record BalanceModel(Guid Id, string Name, decimal Amount, CurrencyModel Currency)
{
    public BalanceModel(Guid id, string name, decimal amount) : this(id, name, amount, null!)
    {
    }
}
