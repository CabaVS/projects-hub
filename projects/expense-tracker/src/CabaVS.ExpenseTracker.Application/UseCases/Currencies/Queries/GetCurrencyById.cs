using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Errors;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.UseCases.Currencies.Queries;

public sealed record GetCurrencyByIdQuery(Guid CurrencyId) 
    : IRequest<Result<CurrencyModel>>;

internal sealed class GetCurrencyByIdQueryHandler(ICurrencyReadRepository currencyReadRepository) 
    : IRequestHandler<GetCurrencyByIdQuery, Result<CurrencyModel>>
{
    public async Task<Result<CurrencyModel>> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        CurrencyModel? currencyModel = await currencyReadRepository.GetCurrencyByIdAsync(
            request.CurrencyId, cancellationToken);
        return currencyModel is not null
            ? currencyModel
            : CurrencyErrors.NotFound(request.CurrencyId);
    }
}
