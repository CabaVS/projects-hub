using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.WriteRepositories;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    IUserWriteRepository Users { get; }
    
    IWorkspaceWriteRepository Workspaces { get; }
    
    ICurrencyWriteRepository Currencies { get; }
    
    IBalanceWriteRepository Balances { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
