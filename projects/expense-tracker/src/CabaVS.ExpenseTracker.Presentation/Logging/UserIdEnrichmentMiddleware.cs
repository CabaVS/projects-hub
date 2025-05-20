using CabaVS.ExpenseTracker.Application.Abstractions.UserContext;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace CabaVS.ExpenseTracker.Presentation.Logging;

internal sealed class UserIdEnrichmentMiddleware(ICurrentUserAccessor currentUserAccessor) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (currentUserAccessor.TryGetCurrentUser(out UserModel currentUser))
        {
            using (LogContext.PushProperty("UserId", currentUser.Id))
            {
                await next(context);
            }
        }
        else
        {
            await next(context);
        }
    }
}
