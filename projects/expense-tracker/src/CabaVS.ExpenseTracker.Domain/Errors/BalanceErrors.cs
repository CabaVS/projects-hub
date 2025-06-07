using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class BalanceErrors
{
    public static NotFoundError NotFound(Guid id) => 
        new(nameof(Balance), $"Balance not found by Id '{id}'.");

    public static Error NameIsNullOrWhitespace() =>
        StringError.IsNullOrWhitespace(nameof(Balance), nameof(Balance.Name));
    public static Error NameIsTooLong(string? value) => 
        StringError.IsTooLong(nameof(Balance), nameof(Balance.Name), BalanceName.MaxLength, value);
}
