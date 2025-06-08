using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.UseCases.Balances.Commands;

public sealed record UpdateBalanceCommand(Guid WorkspaceId, Guid BalanceId, string Name, decimal Amount)
    : IRequest<Result>, IWorkspaceAuthorizationRequest;

internal sealed class UpdateBalanceCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateBalanceCommand, Result>
{
    public async Task<Result> Handle(UpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        Balance? balance = await unitOfWork.Balances.GetByIdAsync(request.BalanceId, cancellationToken);
        if (balance is null)
        {
            return BalanceErrors.NotFound(request.BalanceId);
        }

        Result renameResult = balance.Rename(request.Name);
        if (renameResult.IsFailure)
        {
            return renameResult.Error;
        }

        Result changeAmountResult = balance.ChangeAmount(request.Amount);
        if (changeAmountResult.IsFailure)
        {
            return changeAmountResult.Error;
        }
        
        await unitOfWork.Balances.UpdateAsync(request.WorkspaceId, balance, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
            
        return Result.Success();
    }
}
