using Application.Errors.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Serialization.Results
{
    public class SerializationResultConflictError : IResultSerializationStrategy
    {
        public bool MustExecute(Result result)
        {
            return result.Errors.Any(e => e is ConflictError);
        }

        public bool MustExecute<TContent>(Result<TContent> result)
        {
            return MustExecute(result.ToResult());
        }

        public IActionResult Execute(Result result)
        {
            var error = (ConflictError)result.Errors.First(e => e is ConflictError);

            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                Title = "A specified resource exist in database.",
                Detail = error.Message
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = (int)HttpStatusCode.Conflict
            };
        }

        public IActionResult Execute<TContent>(Result<TContent> result)
        {
           return Execute(result.ToResult());
        }

    }
}
