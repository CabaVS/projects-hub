using System.Diagnostics.CodeAnalysis;
using System.Net;
using CabaVS.ExpenseTracker.Application.UseCases.Balances.Commands;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Balances;

internal sealed class UpdateBalanceEndpoint(ISender sender)
    : Endpoint<
        UpdateBalanceEndpoint.RequestModel,
        Results<
            Ok,
            BadRequest<Error>,
            NotFound<NotFoundError>>>
{
    public override void Configure()
    {
        Put("/api/workspaces/{workspaceId:guid}/balances/{balanceId:guid}");
        Options(x =>
        {
            x.WithName(nameof(UpdateBalanceEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }

    public override async Task<Results<Ok, BadRequest<Error>, NotFound<NotFoundError>>> ExecuteAsync(RequestModel req,
        CancellationToken ct)
    {
        var command = new UpdateBalanceCommand(req.WorkspaceId, req.BalanceId, req.Name, req.Amount);
        
        Result result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid WorkspaceId, Guid BalanceId, string Name, decimal Amount);
    
    [SuppressMessage(
        "Major Code Smell",
        "S1144:Unused private types or members should be removed", 
        Justification = "Type used implicitly.")]
    internal sealed class EndpointSummary : Summary<UpdateBalanceEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Update Balance";
            Description = "Updates a Balance under the Workspace. Current user should have access to the Workspace.";
            
            Params[nameof(RequestModel.WorkspaceId)] = "Workspace Id to filter by.";
            Params[nameof(RequestModel.BalanceId)] = "Balance Id to search by.";
            
            ExampleRequest =
                new RequestModel(
                    Guid.Empty,
                    Guid.Empty,
                    "Test Balance UPD",
                    200.99m);

            Response(
                (int)HttpStatusCode.OK,
                "OK response without a body.");
            
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
