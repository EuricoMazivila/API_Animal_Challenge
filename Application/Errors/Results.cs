using Application.Errors.Errors;
using FluentResults;

namespace Application.Errors;

public static class Results
{
    public static Result ApplicationError(string message)
    {
        return Result.Fail(new ApplicationError(message));
    }

    public static Result InternalError(string messageError = "Unknown error", Exception? exception = default)
    {
        Error error = new InternalError(messageError);

        if (exception is not null) 
            error = error.CausedBy(exception);
        
        return Result.Fail(error);
    }
    
    public static Result ValidationError(Dictionary<string, string[]> dictionaryFieldReasons)
    {
        return Result.Fail(new ValidationError(dictionaryFieldReasons));
    }
    
    public static Result<TContent> ApplicationError<TContent>(string message)
    {
        return Result.Fail<TContent>(new ApplicationError(message));
    }
    
    public static Result<TContent> InternalError<TContent>(string messageError = "Unknown error", Exception? exception = default)
    {
        Error error = new InternalError(messageError);

        if (exception is not null) 
            error = error.CausedBy(exception);
        
        return Result.Fail<TContent>(error);
    }
}