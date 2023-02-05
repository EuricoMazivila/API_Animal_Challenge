using FluentResults;

namespace Application.Errors.Errors;

public class InternalError : Error
{
    public InternalError(string error = "Unknown error", Exception? exception = default) : base(error)
    {
        if (exception is not null)
            CausedBy(exception);
    }
}