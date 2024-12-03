using Microsoft.AspNetCore.Http;
using System.Net;
// using Microsoft.OpenApi.Expressions;
using ShoesStor.Services;

namespace ShoesStor.Middlewares;

public class OurMiddleware
{
    private readonly RequestDelegate _next;

    public OurMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public object JwtTokenValidator { get; private set; }


    //טעינת הדף הראשי רק אם התוקן תקף
    public async Task Invoke(HttpContext context)
    {
        var token=context.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrEmpty(token))
        {
            if (UserTokenServices.IsTokenExpired(token))
            {
                context.Response.Redirect("/index");
                return;
            }
        }
        await _next(context);
    }
}

public static partial class MiddleExtensions
{
    public static IApplicationBuilder UseTokenExpMiddleware(this IApplicationBuilder builder )
    {
        return builder.UseMiddleware<OurMiddleware>();
    }
}

