using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class BalanceName : ValueObject
{
    public const int MaxLength = 32;
    
    public string Value { get; }

    private BalanceName(string value) => Value = value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<BalanceName> Create(string balanceName) =>
        Result<string>.Success(balanceName)
            .EnsureStringNotNullOrWhitespace(BalanceErrors.NameIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, BalanceErrors.NameIsTooLong(balanceName))
            .Map(x => new BalanceName(x));
}
