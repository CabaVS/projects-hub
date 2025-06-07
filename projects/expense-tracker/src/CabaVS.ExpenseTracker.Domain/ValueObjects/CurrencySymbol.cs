using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class CurrencySymbol : ValueObject
{
    public const int MaxLength = 4;
    
    public string Value { get; }

    private CurrencySymbol(string value) => Value = value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<CurrencySymbol> Create(string currencySymbol) =>
        Result<string>.Success(currencySymbol)
            .EnsureStringNotNullOrWhitespace(CurrencyErrors.SymbolIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, CurrencyErrors.SymbolIsTooLong(currencySymbol))
            .Map(x => new CurrencySymbol(x));
}
