using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class CurrencyName : ValueObject
{
    public const int MaxLength = 32;
    
    public string Value { get; }

    private CurrencyName(string value) => Value = value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<CurrencyName> Create(string currencyName) =>
        Result<string>.Success(currencyName)
            .EnsureStringNotNullOrWhitespace(CurrencyErrors.NameIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, CurrencyErrors.NameIsTooLong(currencyName))
            .Map(x => new CurrencyName(x));
}
