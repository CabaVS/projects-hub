using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.WriteRepositories;

public interface ICurrencyWriteRepository
{
    Task<Currency?> GetByIdAsync(Guid currencyId, CancellationToken cancellationToken = default);
}
