using System.Diagnostics.CodeAnalysis;
using System.Net;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Application.UseCases.Workspaces.Queries;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Workspaces;

internal sealed class GetAllWorkspacesEndpoint(ISender sender)
    : EndpointWithoutRequest<
        Results<
            Ok<GetAllWorkspacesEndpoint.ResponseModel>,
            BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/api/workspaces");
        Options(x =>
        {
            x.WithName(nameof(GetAllWorkspacesEndpoint));
            x.WithTags(EndpointTags.Workspaces);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>>> ExecuteAsync(CancellationToken ct)
    {
        var query = new GetAllWorkspacesQuery();
        
        Result<WorkspaceSlimModel[]> result = await sender.Send(query, ct);
        
        return result.ToDefaultApiResponse(workspaces => new ResponseModel(workspaces));
    }

    internal sealed record ResponseModel(WorkspaceSlimModel[] Workspaces);
    
    [SuppressMessage(
        "Major Code Smell",
        "S1144:Unused private types or members should be removed", 
        Justification = "Type used implicitly.")]
    internal sealed class EndpointSummary : Summary<GetAllWorkspacesEndpoint>
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
                        new WorkspaceSlimModel(
                            new Guid("00000001-0001-0001-0001-000000000001"),
                            "Test Workspace 1"),
                        new WorkspaceSlimModel(
                            new Guid("00000001-0001-0001-0001-000000000002"),
                            "Test Workspace 2")
                    ]));
            
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error("Error.Unknown", "Unknown error occured."));
        }
    }
}
