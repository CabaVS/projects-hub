using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.UseCases.Balances.Commands;

public sealed record DeleteBalanceCommand(Guid WorkspaceId, Guid BalanceId)
    : IRequest<Result>, IWorkspaceAuthorizationRequest;

internal sealed class DeleteBalanceCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteBalanceCommand, Result>
{
    public async Task<Result> Handle(DeleteBalanceCommand request, CancellationToken cancellationToken)
    {
        Balance? balance = await unitOfWork.Balances.GetByIdAsync(request.BalanceId, cancellationToken);
        if (balance is null)
        {
            return BalanceErrors.NotFound(request.BalanceId);
        }
        
        await unitOfWork.Balances.DeleteAsync(request.WorkspaceId, balance, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
            
        return Result.Success();
    }
}
