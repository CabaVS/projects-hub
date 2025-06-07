using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class CurrencyCode : ValueObject
{
    public const int MaxLength = 4;
    public const int MinLength = 3;
    
    public string Value { get; }

    private CurrencyCode(string value) => Value = value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<CurrencyCode> Create(string currencyCode) =>
        Result<string>.Success(currencyCode)
            .EnsureStringNotNullOrWhitespace(CurrencyErrors.CodeIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, CurrencyErrors.CodeIsTooLong(currencyCode))
            .EnsureStringNotTooShort(MinLength, CurrencyErrors.CodeIsTooShort(currencyCode))
            .Map(x => new CurrencyCode(x));
}
