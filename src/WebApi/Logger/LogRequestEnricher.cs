using System.Linq;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Continental.API.WebApi.Logger
{
    public static class LogRequestEnricher
    {
        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress);
            diagnosticContext.Set("Host", httpContext.Request.Host);
            diagnosticContext.Set("Path", httpContext.Request.Path);
            diagnosticContext.Set("Metodo", httpContext.Request.Method);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
        }
    }
}
