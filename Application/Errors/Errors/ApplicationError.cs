using FluentResults;

namespace Application.Errors.Errors;

public class ApplicationError : Error
{
    public ApplicationError(string message): base(message)
    {
    }
}