using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Errors;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.UseCases.Balances.Queries;

public sealed record GetBalanceByIdQuery(Guid WorkspaceId, Guid BalanceId) 
    : IWorkspaceAuthorizationRequest, IRequest<Result<BalanceModel>>;

internal sealed class GetWorkspaceByIdQueryHandler(IBalanceReadRepository balanceReadRepository) 
    : IRequestHandler<GetBalanceByIdQuery, Result<BalanceModel>>
{
    public async Task<Result<BalanceModel>> Handle(GetBalanceByIdQuery request, CancellationToken cancellationToken)
    {
        BalanceModel? balanceModel = await balanceReadRepository.GetBalanceByIdAsync(
            request.BalanceId, request.WorkspaceId, cancellationToken);
        return balanceModel is not null
            ? balanceModel
            : BalanceErrors.NotFound(request.BalanceId);
    }
}

