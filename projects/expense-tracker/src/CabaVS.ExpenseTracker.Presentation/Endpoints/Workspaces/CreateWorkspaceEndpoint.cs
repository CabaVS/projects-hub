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

internal sealed class CreateWorkspaceEndpoint(ISender sender)
    : Endpoint<
        CreateWorkspaceEndpoint.RequestModel,
        Results<
            CreatedAtRoute,
            BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/api/workspaces");
        Options(x =>
        {
            x.WithName(nameof(CreateWorkspaceEndpoint));
            x.WithTags(EndpointTags.Workspaces);
        });
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(RequestModel req,
        CancellationToken ct)
    {
        var command = new CreateWorkspaceCommand(req.Name);
        
        Result<Guid> result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse(nameof(GetWorkspaceByIdEndpoint), workspaceId => new { workspaceId });
    }

    internal sealed record RequestModel(string Name);
    
    [SuppressMessage(
        "Major Code Smell",
        "S1144:Unused private types or members should be removed", 
        Justification = "Type used implicitly.")]
    internal sealed class EndpointSummary : Summary<CreateWorkspaceEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Create Workspace";
            Description = "Creates a Workspace. Current user is added as an administrator.";
            
            ExampleRequest =
                new RequestModel(
                    "Test Workspace");

            Response(
                (int)HttpStatusCode.Created,
                "Created response with Location header but without a body.");
            
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error("Error.Unknown", "Unknown error occured."));
        }
    }
}
