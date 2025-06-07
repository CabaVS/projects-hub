namespace CabaVS.ExpenseTracker.Application.Models;

public sealed record BalanceModel(Guid Id, string Name, decimal Amount, CurrencyModel Currency);
