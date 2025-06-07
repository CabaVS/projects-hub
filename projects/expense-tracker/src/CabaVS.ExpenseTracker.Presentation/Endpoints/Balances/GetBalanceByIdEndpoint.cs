using System.Diagnostics.CodeAnalysis;
using System.Net;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Application.UseCases.Balances.Queries;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Balances;

internal sealed class GetBalanceByIdEndpoint(ISender sender)
    : Endpoint<
        GetBalanceByIdEndpoint.RequestModel,
        Results<
            Ok<GetBalanceByIdEndpoint.ResponseModel>,
            BadRequest<Error>,
            NotFound<NotFoundError>>>
{
    public override void Configure()
    {
        Get("/api/workspaces/{workspaceId:guid}/balances/{balanceId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetBalanceByIdEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>, NotFound<NotFoundError>>> ExecuteAsync(RequestModel req, CancellationToken ct)
    {
        var query = new GetBalanceByIdQuery(req.WorkspaceId, req.BalanceId);
        
        Result<BalanceModel> result = await sender.Send(query, ct);
        
        return result.ToDefaultApiResponseWithNotFound(balance => new ResponseModel(balance));
    }

    internal sealed record RequestModel(Guid WorkspaceId, Guid BalanceId);
    internal sealed record ResponseModel(BalanceModel Balance);
    
    [SuppressMessage(
        "Major Code Smell",
        "S1144:Unused private types or members should be removed", 
        Justification = "Type used implicitly.")]
    internal sealed class EndpointSummary : Summary<GetBalanceByIdEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get Balance by Id";
            Description = "Gets a Balance by Id within a Workspace. User should have access to the Workspace.";
            
            Params[nameof(RequestModel.WorkspaceId)] = "Workspace Id to filter by.";
            Params[nameof(RequestModel.BalanceId)] = "Balance Id to search by.";

            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                    new BalanceModel(
                        new Guid("00000001-0001-0001-0001-000000000001"),
                        "Test Workspace",
                        200.99m,
                        new CurrencyModel(
                            new Guid("00000002-0001-0001-0001-000000000001"),
                            "United States Dollar",
                            "USD",
                            "$"))));
            
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
