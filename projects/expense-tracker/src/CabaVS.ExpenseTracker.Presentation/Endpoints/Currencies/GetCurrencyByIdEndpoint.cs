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

internal sealed class GetCurrencyByIdEndpoint(ISender sender)
    : Endpoint<
        GetCurrencyByIdEndpoint.RequestModel,
        Results<
            Ok<GetCurrencyByIdEndpoint.ResponseModel>,
            BadRequest<Error>,
            NotFound<NotFoundError>>>
{
    public override void Configure()
    {
        Get("/api/currencies/{currencyId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetCurrencyByIdEndpoint));
            x.WithTags(EndpointTags.Currencies);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>, NotFound<NotFoundError>>> ExecuteAsync(RequestModel req,
        CancellationToken ct)
    {
        var query = new GetCurrencyByIdQuery(req.CurrencyId);
        
        Result<CurrencyModel> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponseWithNotFound(currency => new ResponseModel(currency));
    }

    internal sealed record RequestModel(Guid CurrencyId);
    internal sealed record ResponseModel(CurrencyModel Currency);
    
    [SuppressMessage(
        "Major Code Smell",
        "S1144:Unused private types or members should be removed", 
        Justification = "Type used implicitly.")]
    internal sealed class EndpointSummary : Summary<GetCurrencyByIdEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get Currency by Id";
            Description = "Gets a Currency by Id.";
            
            Params[nameof(RequestModel.CurrencyId)] = "Currency Id to search by.";

            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                    new CurrencyModel(
                        new Guid("00000001-0001-0001-0001-000000000001"),
                        "United States Dollar",
                        "USD",
                        "$")));
            
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error("Error.Unknown", "Unknown error occured."));
            
            Response(
                (int)HttpStatusCode.NotFound,
                "Not Found with Error.",
                example: new Error("Entity.NotFound", "Entity not found."));
        }
    }
}
