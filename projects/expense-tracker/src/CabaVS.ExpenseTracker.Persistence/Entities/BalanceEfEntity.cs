using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class BalanceEfEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }

    public Guid CurrencyId { get; set; }
    public CurrencyEfEntity? Currency { get; set; }

    public Guid WorkspaceId { get; set; }
    public WorkspaceEfEntity? Workspace { get; set; }
    
    public static BalanceEfEntity ConvertFromDomain(Balance balance) => 
        new()
        {
            Id = balance.Id,
            Name = balance.Name.Value,
            Amount = balance.Amount,
            CurrencyId = balance.Currency.Id
        };

    public Balance ConvertToDomain()
    {
        Balance balance = Balance
            .CreateExisting(
                Id, 
                Name, 
                Amount,
                Domain.Entities.Currency
                    .CreateExisting(Currency!.Id, Currency.Name, Currency.Code, Currency.Symbol)
                    .Value)
            .Value;
        return balance;
    }
}
