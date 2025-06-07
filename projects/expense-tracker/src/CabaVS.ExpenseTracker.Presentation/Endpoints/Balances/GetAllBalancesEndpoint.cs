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

internal sealed class GetAllBalancesEndpoint(ISender sender)
    : Endpoint<
        GetAllBalancesEndpoint.RequestModel,
        Results<
            Ok<GetAllBalancesEndpoint.ResponseModel>,
            BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("/api/workspaces/{workspaceId:guid}/balances");
        Options(x =>
        {
            x.WithName(nameof(GetAllBalancesEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>>> ExecuteAsync(RequestModel req, CancellationToken ct)
    {
        var query = new GetAllBalancesQuery(req.WorkspaceId);
        
        Result<BalanceModel[]> result = await sender.Send(query, ct);
        
        return result.ToDefaultApiResponse(balances => new ResponseModel(balances));
    }

    internal sealed record RequestModel(Guid WorkspaceId);
    internal sealed record ResponseModel(BalanceModel[] Balances);
    
    [SuppressMessage(
        "Major Code Smell",
        "S1144:Unused private types or members should be removed", 
        Justification = "Type used implicitly.")]
    internal sealed class EndpointSummary : Summary<GetAllBalancesEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get all Balances";
            Description = "Gets all Balances within a Workspace. User should have access to the Workspace. Sorted by Name.";
            
            Params[nameof(RequestModel.WorkspaceId)] = "Workspace Id to filter by.";

            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                    [
                        new BalanceModel(
                            new Guid("00000001-0001-0001-0001-000000000001"),
                            "Test Balance 1",
                            200.99m,
                            new CurrencyModel(
                                new Guid("00000002-0001-0001-0001-000000000001"),
                                "United States Dollar",
                                "USD",
                                "$")),
                        new BalanceModel(
                            new Guid("00000001-0001-0001-0001-000000000002"),
                            "Test Balance 2",
                            -100.99m,
                            new CurrencyModel(
                                new Guid("00000002-0001-0001-0001-000000000001"),
                                "United States Dollar",
                                "USD",
                                "$"))
                    ]));
            
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error("Error.Unknown", "Unknown error occured."));
        }
    }
}
