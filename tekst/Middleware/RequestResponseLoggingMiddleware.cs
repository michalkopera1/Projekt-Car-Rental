using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
       
        var request = await FormatRequest(context.Request);
        Console.WriteLine($"Request: {request}");

        
        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        
        var response = await FormatResponse(context.Response);
        Console.WriteLine($"Response: {response}");

        await responseBody.CopyToAsync(originalBodyStream);
    }

    private async Task<string> FormatRequest(HttpRequest request)
    {
        request.EnableBuffering();
        var body = request.Body;

        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        await body.ReadAsync(buffer, 0, buffer.Length);
        var bodyAsText = Encoding.UTF8.GetString(buffer);
        request.Body.Position = 0;

        return $"{request.Method} {request.Path}{request.QueryString} {bodyAsText}";
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return $"StatusCode: {response.StatusCode}, Body: {text}";
    }
}
