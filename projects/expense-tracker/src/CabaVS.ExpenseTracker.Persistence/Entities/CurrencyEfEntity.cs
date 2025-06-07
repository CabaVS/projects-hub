using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

public class CurrencyEfEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Symbol { get; set; } = null!;
    
    public static CurrencyEfEntity ConvertFromDomain(Currency currency) => 
        new()
        {
            Id = currency.Id,
            Name = currency.Name.Value,
            Code = currency.Code.Value,
            Symbol = currency.Symbol.Value
        };

    public Currency ConvertToDomain()
    {
        Currency currency = Currency.CreateExisting(Id, Name, Code, Symbol).Value;
        return currency;
    }
}
