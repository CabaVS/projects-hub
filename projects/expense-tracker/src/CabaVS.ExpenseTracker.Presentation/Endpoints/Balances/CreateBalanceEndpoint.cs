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

internal sealed class CreateBalanceEndpoint(ISender sender)
    : Endpoint<
        CreateBalanceEndpoint.RequestModel,
        Results<
            CreatedAtRoute,
            BadRequest<Error>>>
{
    public override void Configure()
    {
        Post("/api/workspaces/{workspaceId:guid}/balances");
        Options(x =>
        {
            x.WithName(nameof(CreateBalanceEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(RequestModel req,
        CancellationToken ct)
    {
        var command = new CreateBalanceCommand(req.WorkspaceId, req.Name, req.Amount, req.CurrencyId);
        
        Result<Guid> result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse(nameof(GetBalanceByIdEndpoint), balanceId => new { balanceId });
    }

    internal sealed record RequestModel(Guid WorkspaceId, string Name, decimal Amount, Guid CurrencyId);
    
    [SuppressMessage(
        "Major Code Smell",
        "S1144:Unused private types or members should be removed", 
        Justification = "Type used implicitly.")]
    internal sealed class EndpointSummary : Summary<CreateBalanceEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Create Balance";
            Description = "Creates a Balance under the Workspace. Current user should have access to the Workspace.";
            
            Params[nameof(RequestModel.WorkspaceId)] = "Workspace Id to filter by.";
            
            ExampleRequest =
                new RequestModel(
                    Guid.Empty,
                    "Test Balance",
                    200.99m,
                    new Guid("00000002-0001-0001-0001-000000000001"));

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
