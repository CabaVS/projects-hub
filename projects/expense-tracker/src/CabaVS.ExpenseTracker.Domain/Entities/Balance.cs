using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Balance : Entity
{
    public BalanceName Name { get; private set; }
    public decimal Amount { get; private set; }
    public Currency Currency { get; }
    
    private Balance(Guid id, BalanceName name, decimal amount, Currency currency) : base(id)
    {
        Name = name;
        Amount = amount;
        Currency = currency;
    }

    public static Result<Balance> CreateNew(string name, decimal amount, Currency currency) =>
        CreateExisting(Guid.NewGuid(), name, amount, currency);

    public static Result<Balance> CreateExisting(Guid id, string name, decimal amount, Currency currency) =>
        BalanceName.Create(name)
            .Map(x => new Balance(id, x, amount, currency));
}
