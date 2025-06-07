using System.Diagnostics.CodeAnalysis;
using System.Net;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Application.UseCases.Currencies.Queries;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Currencies;

internal sealed class GetAllCurrenciesEndpoint(ISender sender)
    : EndpointWithoutRequest<
        Results<
            Ok<GetAllCurrenciesEndpoint.ResponseModel>,
            BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("/api/currencies");
        Options(x =>
        {
            x.WithName(nameof(GetAllCurrenciesEndpoint));
            x.WithTags(EndpointTags.Currencies);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>>> ExecuteAsync(CancellationToken ct)
    {
        var query = new GetAllCurrenciesQuery();
        
        Result<CurrencyModel[]> result = await sender.Send(query, ct);
        
        return result.ToDefaultApiResponse(currencies => new ResponseModel(currencies));
    }

    internal sealed record ResponseModel(CurrencyModel[] Currencies);
    
    [SuppressMessage(
        "Major Code Smell",
        "S1144:Unused private types or members should be removed", 
        Justification = "Type used implicitly.")]
    internal sealed class EndpointSummary : Summary<GetAllCurrenciesEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get all Workspaces";
            Description = "Gets all Workspaces which are accessible by current user. Sorted by Name.";

            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                [
                    new CurrencyModel(
                        new Guid("00000001-0001-0001-0001-000000000001"),
                        "United States Dollar",
                        "USD",
                        "$"),
                    new CurrencyModel(
                        new Guid("00000001-0001-0001-0001-000000000002"),
                        "Euro",
                        "EUR",
                        "€")
                ]));
            
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error("Error.Unknown", "Unknown error occured."));
        }
    }
}
