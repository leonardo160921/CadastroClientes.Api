using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CadastroClientes.Api.Health;

public static class HealthCheckResponseWriter
{
    public static async Task WriteResponse(
        HttpContext context,
        HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            Status = report.Status.ToString(),
            TotalDuration = report.TotalDuration.TotalMilliseconds,
            Checks = report.Entries.Select(x => new
            {
                Nome = x.Key,
                Status = x.Value.Status.ToString(),
                Duracao = x.Value.Duration.TotalMilliseconds
            })
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}