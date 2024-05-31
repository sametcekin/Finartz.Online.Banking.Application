using System.Net;
using System.Text.Json.Serialization;

namespace Domain.Abstractions;

public class Result<T>
{
    public T Data { get; set; }

    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    public string Message { get; set; }

    public string Description { get; private set; }

    public Result()
    {
    }
    protected Result(HttpStatusCode statusCode, string? message = null)
    {
        StatusCode = statusCode;
        Message = string.IsNullOrEmpty(message) ? statusCode.ToString() : message.Trim();
    }
    public static Result<T> Success(T data)
    {
        return new Result<T>
        {
            StatusCode = HttpStatusCode.OK,
            Data = data
        };
    }

    public static Result<T> Success(T data, string message)
    {
        return new Result<T>
        {
            StatusCode = HttpStatusCode.OK,
            Data = data,
            Message = message
        };
    }

    public static Result<T> Unauthorized(string message = "INVALID_CREDENTIALS")
    {
        return new Result<T>
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Message = message
        };
    }

    public static Result<T> Unauthorized(string message, string description = "INVALID_CREDENTIALS")
    {
        return new Result<T>
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Message = message,
            Description = description
        };
    }

    public static Result<T> Failure(string message)
    {
        return new Result<T>
        {
            StatusCode = HttpStatusCode.BadRequest,
            Message = message
        };
    }

    public static Result<T> Exception(string message)
    {
        return new Result<T>
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Message = message
        };
    }

    public static Result<T> Failure(string message, string description)
    {
        return new Result<T>
        {
            StatusCode = HttpStatusCode.BadRequest,
            Message = message,
            Description = description,
        };
    }

    public static Result<T> Failure(string message, string description, string code)
    {
        return new Result<T>
        {
            StatusCode = HttpStatusCode.BadRequest,
            Message = message,
            Description = description,
        };
    }

    public static Result<T> ErrorWithCode(string message, string code)
    {
        return new Result<T>
        {
            StatusCode = HttpStatusCode.BadRequest,
            Message = message,
        };
    }

    public static Result<T> Failure(T data, string message)
    {
        return new Result<T>
        {
            StatusCode = HttpStatusCode.BadRequest,
            Data = data,
            Message = message
        };
    }

    public static Result<T> Error(T data)
    {
        return new Result<T>
        {
            StatusCode = HttpStatusCode.BadRequest,
            Data = data
        };
    }

    public static Result<T> Error(IEnumerable<string> errors)
    {
        return new Result<T>
        {
            StatusCode = HttpStatusCode.BadRequest,
            Message = string.Join(',', errors)
        };
    }

    public static Result<T> Error(IEnumerable<string> errors, string description)
    {
        return new Result<T>
        {
            StatusCode = HttpStatusCode.BadRequest,
            Message = string.Join(',', errors),
            Description = description
        };
    }

    public bool IsFailure() => !IsSuccess();
    public bool IsUnauthorized() => StatusCode is HttpStatusCode.Unauthorized;
    public bool IsSuccess() => StatusCode is HttpStatusCode.OK;
    public bool IsException() => StatusCode is HttpStatusCode.InternalServerError;
}
