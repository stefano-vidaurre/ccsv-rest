using CCSV.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using CCSV.Domain.Repositories.Exceptions;

namespace CCSV.Rest.Exceptions;

public class HttpStatusCodeExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpStatusCodeExceptionHandler> _logger;

    public HttpStatusCodeExceptionHandler(RequestDelegate next, ILogger<HttpStatusCodeExceptionHandler> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidValueException ex)
        {
            await ReportInformation(context, ex, StatusCodes.Status400BadRequest);
        }
        catch (NotAllowedOperationException ex)
        {
            await ReportInformation(context, ex, StatusCodes.Status400BadRequest);
        }
        catch (WrongOperationException ex)
        {
            await ReportInformation(context, ex, StatusCodes.Status400BadRequest);
        }
        catch (DuplicatedValueException ex)
        {
            await ReportInformation(context, ex, StatusCodes.Status400BadRequest);
        }
        catch (ValueNotFoundException ex)
        {
            await ReportInformation(context, ex, StatusCodes.Status404NotFound);
        }
        catch (BadGatewayException ex)
        {
            await ReportWarning(context, ex, StatusCodes.Status502BadGateway);
        }
        catch (EntityUpdateConcurrencyException ex)
        {
            await ReportWarning(context, ex, StatusCodes.Status409Conflict);
        }
        catch (BusinessException ex)
        {
            await ReportError(context, ex, StatusCodes.Status500InternalServerError);
        }
        catch (InternalRepositoryException ex)
        {
            await ReportError(context, ex, StatusCodes.Status500InternalServerError);
        }
        catch (DomainException ex)
        {
            await ReportError(context, ex, StatusCodes.Status500InternalServerError);
        }
        catch (Exception ex)
        {
            await ReportCritical(context, ex, StatusCodes.Status500InternalServerError);
        }
    }

    private async Task ReportInformation(HttpContext context, Exception ex, int status)
    {
        ProblemDetails problemDetail = CreateProblemDetails(ex, status);

        _logger.LogInformation(ex, ex.Message);
        await SendHttpStatusCode(context, problemDetail);
    }

    private async Task ReportWarning(HttpContext context, Exception ex, int status)
    {
        ProblemDetails problemDetail = CreateProblemDetails(ex, status);

        _logger.LogWarning(ex, ex.Message);
        await SendHttpStatusCode(context, problemDetail);
    }

    private async Task ReportError(HttpContext context, Exception ex, int status)
    {
        ProblemDetails problemDetail = CreateProblemDetails(ex, status);

        _logger.LogError(ex, ex.Message);
        await SendHttpStatusCode(context, problemDetail);
    }

    private async Task ReportCritical(HttpContext context, Exception ex, int status)
    {
        ProblemDetails problemDetail = CreateProblemDetails(ex, status);

        _logger.LogCritical(ex, ex.Message);
        await SendHttpStatusCode(context, problemDetail);
    }

    private async Task SendHttpStatusCode(HttpContext context, ProblemDetails problemDetail)
    {
        if (context.Response.HasStarted)
        {
            _logger.LogError("The response has already started, the http status code middleware will not be executed.");
            return;
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problemDetail.Status ?? 0;

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetail));
    }

    private static ProblemDetails CreateProblemDetails(Exception ex, int status)
    {
        return new ProblemDetails() {
            Status = status,
            Title = ex.GetType().Name,
            Detail = ex.Message
        };
    }

}
