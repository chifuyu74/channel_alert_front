using channel_alert_front.ApiService.Services;
using System.Net;

namespace channel_alert_front.ApiService.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TokenBlackListService _blackListService;

    public JwtMiddleware(RequestDelegate next, TokenBlackListService blackListService)
    {
        _next = next;
        _blackListService = blackListService;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            if (await _blackListService.IsTokenBlackListedAsync(token))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Token is blacklisted");
                return;
            }
        }

        await _next(context);
    }
}
