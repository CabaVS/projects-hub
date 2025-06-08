using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.UseCases.Balances.Commands;

public sealed record CreateBalanceCommand(Guid WorkspaceId, string Name, decimal Amount, Guid CurrencyId)
    : IRequest<Result<Guid>>, IWorkspaceAuthorizationRequest;

internal sealed class CreateBalanceCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateBalanceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateBalanceCommand request, CancellationToken cancellationToken)
    {
        Currency? currency = await unitOfWork.Currencies.GetByIdAsync(request.CurrencyId, cancellationToken);
        if (currency is null)
        {
            return CurrencyErrors.NotFound(request.CurrencyId);
        }
        
        Result<Balance> balanceResult = Balance.CreateNew(request.Name, request.Amount, currency);
        if (balanceResult.IsFailure)
        {
            return balanceResult.Error;
        }
        
        Guid balanceId = await unitOfWork.Balances.AddAsync(request.WorkspaceId, balanceResult.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
            
        return balanceId;
    }
}
