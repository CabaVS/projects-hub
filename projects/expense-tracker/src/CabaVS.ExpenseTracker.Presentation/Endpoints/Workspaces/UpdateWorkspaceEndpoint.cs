using System.Diagnostics.CodeAnalysis;
using System.Net;
using CabaVS.ExpenseTracker.Application.UseCases.Workspaces.Commands;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Workspaces;

internal sealed class UpdateWorkspaceEndpoint(ISender sender)
    : Endpoint<
        UpdateWorkspaceEndpoint.RequestModel,
        Results<
            Ok,
            BadRequest<Error>,
            NotFound<NotFoundError>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Put("/api/workspaces/{workspaceId:guid}");
        Options(x =>
        {
            x.WithName(nameof(UpdateWorkspaceEndpoint));
            x.WithTags(EndpointTags.Workspaces);
        });
    }

    public override async Task<Results<Ok, BadRequest<Error>, NotFound<NotFoundError>>> ExecuteAsync(RequestModel req,
        CancellationToken ct)
    {
        var command = new UpdateWorkspaceCommand(req.WorkspaceId, req.Name);
        
        Result result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid WorkspaceId, string Name);
    
    [SuppressMessage(
        "Major Code Smell",
        "S1144:Unused private types or members should be removed", 
        Justification = "Type used implicitly.")]
    internal sealed class EndpointSummary : Summary<UpdateWorkspaceEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Update Workspace";
            Description = "Updates a Workspace. Only if current user is an administrator over that workspace.";
            
            Params[nameof(RequestModel.WorkspaceId)] = "Workspace Id to search by.";
            
            ExampleRequest =
                new RequestModel(
                    Guid.Empty,
                    "Test Workspace UPD");

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
