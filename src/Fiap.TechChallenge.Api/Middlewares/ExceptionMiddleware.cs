using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.TechChallenge.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(httpContext, ex, logger);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex, logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ExceptionMiddleware> logger)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        logger.LogError($"Error: {exception.Message}");
        
        return context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            context.Response.StatusCode,
            Message = "We have some problems, please try again later.",
        } ));
    }
    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException validationException, ILogger<ExceptionMiddleware> logger)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var errorValidations = validationException.Errors.GroupBy(x => x.PropertyName);
        var dictionaryErrors = errorValidations
            .ToDictionary(x => x.Key,
                x => x.Select(error => error.ErrorMessage).ToArray());
        
        var validationProblemDetails = new ValidationProblemDetails(dictionaryErrors);
        var message = JsonSerializer.Serialize(validationProblemDetails);
        logger.LogError(message);
        return context.Response.WriteAsync(message);
    }

}