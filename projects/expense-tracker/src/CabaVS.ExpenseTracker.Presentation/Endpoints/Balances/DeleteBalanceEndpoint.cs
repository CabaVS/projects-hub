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

internal sealed class DeleteBalanceEndpoint(ISender sender)
    : Endpoint<
        DeleteBalanceEndpoint.RequestModel,
        Results<
            Ok,
            BadRequest<Error>,
            NotFound<NotFoundError>>>
{
    public override void Configure()
    {
        Delete("/api/workspaces/{workspaceId:guid}/balances/{balanceId:guid}");
        Options(x =>
        {
            x.WithName(nameof(DeleteBalanceEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }

    public override async Task<Results<Ok, BadRequest<Error>, NotFound<NotFoundError>>> ExecuteAsync(RequestModel req,
        CancellationToken ct)
    {
        var command = new DeleteBalanceCommand(req.WorkspaceId, req.BalanceId);
        
        Result result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid WorkspaceId, Guid BalanceId);
    
    [SuppressMessage(
        "Major Code Smell",
        "S1144:Unused private types or members should be removed", 
        Justification = "Type used implicitly.")]
    internal sealed class EndpointSummary : Summary<DeleteBalanceEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Delete Balance";
            Description = "Deletes a Balance under the Workspace. Current user should have access to the Workspace.";
            
            Params[nameof(RequestModel.WorkspaceId)] = "Workspace Id to filter by.";
            Params[nameof(RequestModel.BalanceId)] = "Balance Id to search by.";

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
