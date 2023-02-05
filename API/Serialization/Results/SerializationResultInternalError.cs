using System.Net;
using Application.Errors.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Serialization.Results;

public class SerializationResultInternalError : IResultSerializationStrategy
{
    public bool MustExecute(Result result)
    {
        return result.Errors.Any(e => e is InternalError);
    }

    public bool MustExecute<TContent>(Result<TContent> result)
    {
        return MustExecute(result.ToResult());
    }

    public IActionResult Execute(Result result)
    {
        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "An unknown internal error occurred."
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }

    public IActionResult Execute<TContent>(Result<TContent> result)
    {
        return Execute(result.ToResult());
    }
}