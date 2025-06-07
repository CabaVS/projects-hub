using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Domain.Common;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.UseCases.Currencies.Queries;

public sealed record GetAllCurrenciesQuery
    : IRequest<Result<CurrencyModel[]>>;

internal sealed class GetAllCurrenciesQueryHandler(ICurrencyReadRepository currencyReadRepository)
    : IRequestHandler<GetAllCurrenciesQuery, Result<CurrencyModel[]>>
{
    public async Task<Result<CurrencyModel[]>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        CurrencyModel[] currencies = await currencyReadRepository.GetAllCurrenciesAsync(cancellationToken);
        return currencies;
    }
}
