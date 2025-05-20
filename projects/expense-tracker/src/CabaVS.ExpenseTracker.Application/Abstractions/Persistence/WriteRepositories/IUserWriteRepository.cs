using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.WriteRepositories;

public interface IUserWriteRepository
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
