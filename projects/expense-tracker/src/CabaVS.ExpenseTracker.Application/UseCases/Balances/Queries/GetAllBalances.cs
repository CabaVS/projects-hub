using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Domain.Common;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.UseCases.Balances.Queries;

public sealed record GetAllBalancesQuery(Guid WorkspaceId)
    : IRequest<Result<BalanceModel[]>>, IWorkspaceAuthorizationRequest;

internal sealed class GetAllBalancesQueryHandler(IBalanceReadRepository balanceReadRepository)
    : IRequestHandler<GetAllBalancesQuery, Result<BalanceModel[]>>
{
    public async Task<Result<BalanceModel[]>> Handle(GetAllBalancesQuery request, CancellationToken cancellationToken)
    {
        BalanceModel[] balances = await balanceReadRepository.GetAllBalancesAsync(request.WorkspaceId, cancellationToken);
        return balances;
    }
}
