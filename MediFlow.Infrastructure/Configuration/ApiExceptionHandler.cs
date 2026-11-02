using MediFlow.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MediFlow.Domain.Exceptions;

namespace MediFlow.Infrastructure.Configuration;

public class ApiExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        var (status, title) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
            ConflictException => (StatusCodes.Status409Conflict, "Conflicto"),
            BusinessValidationException => (StatusCodes.Status400BadRequest, "Validación fallida"),
            _ => (0, string.Empty)
        };

        if (status == 0) return false; // otros errores siguen su curso normal (500)

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(
            new ProblemDetails { Status = status, Title = title, Detail = exception.Message }, ct);

        return true;
    }
}