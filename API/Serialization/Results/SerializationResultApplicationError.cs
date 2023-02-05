using System.Net;
using Application.Errors.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Serialization.Results;

public class SerializationResultApplicationError : IResultSerializationStrategy
{
    public bool MustExecute(Result result)
    {
        return result.Errors.Any(e => e is ApplicationError);
    }

    public bool MustExecute<TContent>(Result<TContent> result)
    {
        return MustExecute(result.ToResult());
    }

    public IActionResult Execute(Result result)
    {
        var error = (ApplicationError)result.Errors.First(e => e is ApplicationError);

        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "",
            Detail = error.Message
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = (int)HttpStatusCode.Forbidden
        };
    }

    public IActionResult Execute<TContent>(Result<TContent> result)
    {
        return Execute(result.ToResult());
    }
}