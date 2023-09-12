using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using SharedLib.Infrastructure.DTOs;

namespace SharedLib.ResponseWrapper;

/// <summary>
/// Wraps response in a consistent format
/// </summary>
public class ApiResponse
{
    public int StatusCode { get; }
    public bool IsError { get; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; }

    public ApiResponse(int statusCode, bool isError, string? message)
    {
        StatusCode = statusCode;
        IsError = isError;
        Message = message ?? DefaultMessageForStatusCode(statusCode);
    }

    private static string DefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            200 => "Ok",
            201 => "Created",
            202 => "Accepted",
            400 => "Bad request",
            404 => "Not found",
            500 => "Internal server error",
            _ => null
        } ?? throw new InvalidOperationException();
    }
}

/// <inheritdoc cref="ApiResponse"/>
/// <remarks>Contains a result of type <typeparamref name="T"/></remarks>
public class ApiOkResponse<T> : ApiResponse
{
    public virtual T Result { get; }

    public ApiOkResponse(T result, string? message) : base(200, false, message)
    {
        Result = result;
    }
}

public class ApiPaginatedOkResponse<T> : ApiOkResponse<IEnumerable<T>>
{
    public Pagination Pagination { get; set; }

    public ApiPaginatedOkResponse(IEnumerable<T> result, Pagination pagination, string? message) : base(result,
        message)
    {
        Pagination = pagination;
    }
}

/// <inheritdoc cref="ApiResponse"/>
public class ApiNotFoundResponse : ApiResponse
{
    public ApiNotFoundResponse(string? message) : base(404, true, message)
    {
    }
}

/// <inheritdoc cref="ApiResponse"/>
public class ApiBadRequestResponse : ApiResponse
{
    public IReadOnlyDictionary<string, List<string>>? Errors { get; }

    public ApiBadRequestResponse(ModelStateDictionary modelState, string? message) : base(400, true, message)
    {
        Errors = modelState.ToDictionary(e => e.Key, e => e.Value!.Errors.Select(error => error.ErrorMessage).ToList());
    }

    public ApiBadRequestResponse(IEnumerable<ValidationResult> validationResults, string message) : base(400, true,
        message)
    {
        Errors = validationResults.GroupBy(r => r.MemberNames.Single())
            .ToDictionary(e => e.Key, e => e.Select(r => r.ErrorMessage!).ToList());
    }

    public ApiBadRequestResponse(string message) : base(400, true, message)
    {
    }
}

public class ApiInternalServerErrorResponse : ApiResponse
{
    public ApiInternalServerErrorResponse(string message) : base(500, true, message)
    {
    }
}

/// <inheritdoc cref="ApiResponse"/>
public class ApiCreatedResponse<T> : ApiResponse
{
    public T Result { get; }

    public ApiCreatedResponse(T result, string? message) : base(201, false, message)
    {
        Result = result;
    }
}

/// <inheritdoc cref="ApiResponse"/>
public class ApiUnauthorizedResponse : ApiResponse
{
    public ApiUnauthorizedResponse(string message) : base(401, true, message)
    {
    }
}