using System.Net;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Serialization.Results;

public class SerializationResultSuccess : IResultSerializationStrategy
{
    public bool MustExecute(Result result)
    {
        return result.IsSuccess;
    }

    public bool MustExecute<TContent>(Result<TContent> result)
    {
        return result.IsSuccess;
    }

    public IActionResult Execute(Result result)
    {
        return new StatusCodeResult((int)HttpStatusCode.OK);
    }

    public IActionResult Execute<TContent>(Result<TContent> result)
    {
        return new ObjectResult(result.Value)
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}