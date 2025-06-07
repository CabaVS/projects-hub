using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class CurrencyErrors
{
    public static NotFoundError NotFound(Guid id) => 
        new(nameof(Currency), $"Currency not found by Id '{id}'.");

    public static Error NameIsNullOrWhitespace() =>
        StringError.IsNullOrWhitespace(nameof(Currency), nameof(Currency.Name));
    public static Error NameIsTooLong(string? value) => 
        StringError.IsTooLong(nameof(Currency), nameof(Currency.Name), CurrencyName.MaxLength, value);
    
    public static Error CodeIsNullOrWhitespace() =>
        StringError.IsNullOrWhitespace(nameof(Currency), nameof(Currency.Code));
    public static Error CodeIsTooLong(string? value) => 
        StringError.IsTooLong(nameof(Currency), nameof(Currency.Code), CurrencyCode.MaxLength, value);
    public static Error CodeIsTooShort(string? value) => 
        StringError.IsTooShort(nameof(Currency), nameof(Currency.Code), CurrencyCode.MinLength, value);
    
    public static Error SymbolIsNullOrWhitespace() =>
        StringError.IsNullOrWhitespace(nameof(Currency), nameof(Currency.Symbol));
    public static Error SymbolIsTooLong(string? value) => 
        StringError.IsTooLong(nameof(Currency), nameof(Currency.Symbol), CurrencySymbol.MaxLength, value);
}
