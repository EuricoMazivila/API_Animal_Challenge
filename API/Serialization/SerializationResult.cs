using System.Net;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Serialization;

public static class SerializationResult
{
    private static IActionResult SerializeResult(IEnumerable<IResultSerializationStrategy> strategies,
        Result result)
    {
        var strategy = strategies.FirstOrDefault(e => e.MustExecute(result));

        return strategy.Execute(result) ?? new ObjectResult(new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "An unknown internal error occurred."
        })
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }

    private static IActionResult SerializeResult<TContent>(IEnumerable<IResultSerializationStrategy> strategies,
        Result<TContent> result)
    {
        var strategy = strategies.FirstOrDefault(e => e.MustExecute(result));

        return strategy?.Execute(result) ?? new ObjectResult(new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "An unknown internal error occurred."
        })
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }

    public static IActionResult SerializeResult(this ControllerBase controller, Result result)
    {
        var strategies = controller.HttpContext.RequestServices.GetService<IEnumerable<IResultSerializationStrategy>>();
        return SerializeResult(strategies, result);
    }
    
    public static IActionResult SerializeResult<TContent>(this ControllerBase controller, Result<TContent> result)
    {
        var strategies = controller.HttpContext.RequestServices.GetService<IEnumerable<IResultSerializationStrategy>>();
        return SerializeResult(strategies, result);
    }
}