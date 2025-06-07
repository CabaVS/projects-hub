using CabaVS.ExpenseTracker.Application.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;

public interface ICurrencyReadRepository
{
    Task<CurrencyModel[]> GetAllCurrenciesAsync(CancellationToken cancellationToken = default);
    Task<CurrencyModel?> GetCurrencyByIdAsync(Guid currencyId, CancellationToken cancellationToken = default);
}
