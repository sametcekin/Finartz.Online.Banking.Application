using Domain.Abstractions;
using Domain.Exceptions;
using Domain.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace Infrastructure;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param>
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="environment"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext httpContext, IWebHostEnvironment environment)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(httpContext, e);
        }
    }

    private Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var message = exception.Message;
        var exceptionType = exception.GetType();

        var statusCode = exceptionType switch
        {
            _ when exceptionType == typeof(UnauthorizedAccessException) => HttpStatusCode.Forbidden,
            _ when exceptionType == typeof(ArgumentNullException) => HttpStatusCode.BadRequest,
            _ when exceptionType == typeof(UnauthorizedAccessException) => HttpStatusCode.Unauthorized,
            _ when exceptionType == typeof(ValidationException) => HttpStatusCode.BadRequest,
            _ when exceptionType == typeof(BusinessRuleException) => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        if (exception is ValidationException validationException)
        {
            message = validationException.Errors.Select(s => new
            {
                s.PropertyName,
                s.ErrorMessage
            }).AsJson();
        }

        var errorResponse = Result<object>.Failure(message).AsJson();

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = (int)statusCode;
        return httpContext.Response.WriteAsync(errorResponse, Encoding.UTF8);
    }
}
