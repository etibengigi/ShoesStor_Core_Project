using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace ShoesStor.Middlewares;

public class OurLogMiddleware
{
    private RequestDelegate next;

    private readonly string logFilePath;

    public OurLogMiddleware(RequestDelegate next,string logFilePath)
    {
        this.next = next;
        this.logFilePath=logFilePath;
    }

    public async Task Invoke(HttpContext c)
    {
        // await c.Response.WriteAsync($"Our Log Middleware start\n");
        var sw = new Stopwatch();
        sw.Start();
        await next(c);
        // Console.WriteLine($"{c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
        //     + $" User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");
        // await c.Response.WriteAsync("Our Log Middleware end\n");

        WriteLogToFile($"{c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
            + $" User: {c.User?.FindFirst("Id")?.Value ?? "unknown"}");     
    }

        private void WriteLogToFile(string logMessage)
        {
            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                sw.WriteLine(logMessage);
            }
        }
}
public static class OurLogMiddlewareHelper
{
    public static void UseOurLog(
        this IApplicationBuilder a)
    {
        a.UseMiddleware<OurLogMiddleware>();
    }
}
