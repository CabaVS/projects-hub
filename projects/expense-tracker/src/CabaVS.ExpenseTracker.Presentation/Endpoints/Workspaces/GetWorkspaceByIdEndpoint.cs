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

internal sealed class GetWorkspaceByIdEndpoint(ISender sender)
    : Endpoint<
        GetWorkspaceByIdEndpoint.RequestModel,
        Results<
            Ok<GetWorkspaceByIdEndpoint.ResponseModel>,
            BadRequest<Error>,
            NotFound<NotFoundError>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/api/workspaces/{workspaceId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetWorkspaceByIdEndpoint));
            x.WithTags(EndpointTags.Workspaces);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>, NotFound<NotFoundError>>> ExecuteAsync(RequestModel req,
        CancellationToken ct)
    {
        var query = new GetWorkspaceByIdQuery(req.WorkspaceId);
        
        Result<WorkspaceModel> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponseWithNotFound(workspace => new ResponseModel(workspace));
    }

    internal sealed record RequestModel(Guid WorkspaceId);
    internal sealed record ResponseModel(WorkspaceModel Workspace);
    
    [SuppressMessage(
        "Major Code Smell",
        "S1144:Unused private types or members should be removed", 
        Justification = "Type used implicitly.")]
    internal sealed class EndpointSummary : Summary<GetWorkspaceByIdEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get Workspace by Id";
            Description = "Gets a Workspace by Id. Only if current user has access to it.";
            
            Params[nameof(RequestModel.WorkspaceId)] = "Workspace Id to search by.";

            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                    new WorkspaceModel(
                        new Guid("00000001-0001-0001-0001-000000000001"),
                        "Test Workspace",
                        true)));
            
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
