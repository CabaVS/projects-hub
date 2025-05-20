using CabaVS.ExpenseTracker.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Extensions;

internal static class ResultExtensions
{
    internal static Results<Ok, BadRequest<Error>, NotFound<NotFoundError>> ToDefaultApiResponse(this Result result) =>
        result.Match<Results<Ok, BadRequest<Error>, NotFound<NotFoundError>>>(
            () => TypedResults.Ok(),
            error => error is NotFoundError notFoundError
                ? TypedResults.NotFound(notFoundError)
                : TypedResults.BadRequest(error));

    internal static Results<Ok<TResponse>, BadRequest<Error>> ToDefaultApiResponse<TResult, TResponse>(
        this Result<TResult> result,
        Func<TResult, TResponse> mappingFunc) =>
        result.Match<TResult, Results<Ok<TResponse>, BadRequest<Error>>>(
            value => TypedResults.Ok(mappingFunc(value)),
            error => TypedResults.BadRequest(error));
    
    internal static Results<CreatedAtRoute, BadRequest<Error>> ToDefaultApiResponse(
        this Result<Guid> result,
        string routeName,
        Func<Guid, object> mappingFunc) =>
        result.Match<Guid, Results<CreatedAtRoute, BadRequest<Error>>>(
            value => TypedResults.CreatedAtRoute(routeName, mappingFunc(value)),
            error => TypedResults.BadRequest(error));
    
    internal static Results<Ok<TResponse>, BadRequest<Error>, NotFound<NotFoundError>> ToDefaultApiResponseWithNotFound<TResult, TResponse>(
        this Result<TResult> result,
        Func<TResult, TResponse> mappingFunc) =>
        result.Match<TResult, Results<Ok<TResponse>, BadRequest<Error>, NotFound<NotFoundError>>>(
            value => TypedResults.Ok(mappingFunc(value)),
            error => error is NotFoundError notFoundError
                ? TypedResults.NotFound(notFoundError)
                : TypedResults.BadRequest(error));
}
