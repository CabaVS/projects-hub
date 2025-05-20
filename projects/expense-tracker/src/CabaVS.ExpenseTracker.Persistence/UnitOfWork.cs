using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.WriteRepositories;
using CabaVS.ExpenseTracker.Persistence.WriteRepositories;

namespace CabaVS.ExpenseTracker.Persistence;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private readonly Lazy<IUserWriteRepository> _userWriteRepository =
        new(() => new UserWriteRepository(dbContext));
    public IUserWriteRepository Users => _userWriteRepository.Value;
    
    private readonly Lazy<IWorkspaceWriteRepository> _workspaceWriteRepository =
        new(() => new WorkspaceWriteRepository(dbContext));
    public IWorkspaceWriteRepository Workspaces => _workspaceWriteRepository.Value;
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => 
        await dbContext.SaveChangesAsync(cancellationToken);
}
