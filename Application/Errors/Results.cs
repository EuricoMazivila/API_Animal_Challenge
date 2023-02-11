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

    public static Result NotFoundError(string name)
    {
        return Result.Fail(new NotFoundError($"The resource: {name} specified cannot be found"));
    }

    public static Result ConflictError(string message)
    {
        return Result.Fail(new ConflictError($"The resource: {message} exist in database!"));
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

    public static Result<TContent> NotFoundError<TContent>(string name)
    {
        return Result.Fail<TContent>(new NotFoundError($"The resource: {name} specified cannot be found"));
    }

    public static Result<TContent> ConflictError<TContent>(string message)
    {
        return Result.Fail<TContent>(new ConflictError($"The resource: {message} exist in database!"));
    }
}