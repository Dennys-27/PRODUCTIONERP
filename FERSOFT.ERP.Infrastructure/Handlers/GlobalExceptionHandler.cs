using FERSOFT.ERP.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Infrastructure.Handlers;
public class GlobalExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(HttpContext httpContext, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "Ocurrió un error inesperado.";

        // Obtener la excepción
        var exceptionFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionFeature?.Error;

        // Si la excepción es personalizada, cambia el mensaje y el código de estado
        if (exception is CustomException customEx)
        {
            statusCode = customEx.StatusCode;
            message = customEx.Message;
        }
        else
        {
            _logger.LogError(exception, "Excepción no controlada");
        }

        var response = new
        {
            title = "Error",
            status = statusCode,
            message
        };

        httpContext.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(response);
        await httpContext.Response.WriteAsync(json, cancellationToken);
    }
}
