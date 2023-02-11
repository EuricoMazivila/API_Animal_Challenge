using Application.Errors.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Serialization.Results
{
    public class SerializationResultNotFoundError : IResultSerializationStrategy
    {
        public bool MustExecute(Result result)
        {
            return result.Errors.Any(e => e is NotFoundError);
        }

        public bool MustExecute<TContent>(Result<TContent> result)
        {
            return MustExecute(result.ToResult());
        }

        public IActionResult Execute(Result result)
        {
            var error = (NotFoundError)result.Errors.First(e => e is NotFoundError);

            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "A specified resource cannot be found.",
                Detail = error.Message
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = (int)HttpStatusCode.NotFound
            };
        }

        public IActionResult Execute<TContent>(Result<TContent> result)
        {
            return Execute(result.ToResult());
        }

    }
}
