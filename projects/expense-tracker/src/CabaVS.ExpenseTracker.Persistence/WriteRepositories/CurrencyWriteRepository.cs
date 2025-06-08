using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.WriteRepositories;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.WriteRepositories;

internal sealed class CurrencyWriteRepository(ApplicationDbContext dbContext) : ICurrencyWriteRepository
{
    public async Task<Currency?> GetByIdAsync(Guid currencyId, CancellationToken cancellationToken = default)
    {
        CurrencyEfEntity? currencyEf = await dbContext.Currencies
            .AsNoTracking()
            .Where(w => w.Id == currencyId)
            .FirstOrDefaultAsync(cancellationToken);
        return currencyEf?.ConvertToDomain();
    }
}
