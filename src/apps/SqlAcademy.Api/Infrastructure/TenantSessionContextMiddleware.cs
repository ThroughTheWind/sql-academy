using Microsoft.AspNetCore.Mvc;
using SqlAcademy.Persistence.MultiTenancy;

namespace SqlAcademy.Api.Infrastructure;

public sealed class TenantSessionContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ISqlSessionContextAccessor sessionContextAccessor)
    {
        sessionContextAccessor.TenantId = null;
        sessionContextAccessor.BypassRowLevelSecurity = false;

        if (context.Request.Headers.TryGetValue(TenantRequestHeaderNames.TenantId, out var tenantHeaderValue) &&
            !string.IsNullOrWhiteSpace(tenantHeaderValue))
        {
            if (!int.TryParse(tenantHeaderValue.ToString(), out var tenantId))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid tenant header",
                    Detail = $"Header '{TenantRequestHeaderNames.TenantId}' must be a valid integer.",
                    Instance = context.Request.Path,
                });
                return;
            }

            sessionContextAccessor.TenantId = tenantId;
        }

        await next(context);
    }
}