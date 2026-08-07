using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace CadastroClientes.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado na aplicação.");

            var problem = ex switch
            {
                ArgumentException => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Requisição inválida.",
                    Detail = ex.Message,
                    Type = "https://httpstatuses.com/400"
                },

                KeyNotFoundException => new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Recurso não encontrado.",
                    Detail = ex.Message,
                    Type = "https://httpstatuses.com/404"
                },

                UnauthorizedAccessException => new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Acesso não autorizado.",
                    Detail = ex.Message,
                    Type = "https://httpstatuses.com/401"
                },

                _ => new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Erro interno do servidor.",
                    Detail = "Ocorreu um erro inesperado.",
                    Type = "https://httpstatuses.com/500"
                }
            };

            problem.Instance = context.Request.Path;

            problem.Extensions["traceId"] = context.TraceIdentifier;

            context.Response.StatusCode = problem.Status!.Value;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(problem));
        }
    }
}