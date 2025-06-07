using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Currency : Entity
{
    public CurrencyName Name { get; private set; }
    public CurrencyCode Code { get; private set; }
    public CurrencySymbol Symbol { get; private set; }
    
    private Currency(Guid id, CurrencyName name, CurrencyCode code, CurrencySymbol symbol) : base(id)
    {
        Name = name;
        Code = code;
        Symbol = symbol;
    }

    public static Result<Currency> CreateNew(string name, string code, string symbol) =>
        CreateExisting(Guid.NewGuid(), name, code, symbol);

    public static Result<Currency> CreateExisting(Guid id, string name, string code, string symbol)
    {
        CurrencyName currencyName = null!;
        CurrencyCode currencyCode = null!;
        CurrencySymbol currencySymbol = null!;
        
        return Result.Success()
            .Bind(() => CurrencyName.Create(name)).Tap(n => currencyName = n)
            .Bind(() => CurrencyCode.Create(code)).Tap(c => currencyCode = c)
            .Bind(() => CurrencySymbol.Create(symbol)).Tap(s => currencySymbol = s)
            .Map(() => new Currency(id, currencyName, currencyCode, currencySymbol));
    }
}
