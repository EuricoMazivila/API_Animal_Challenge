using FluentResults;

namespace Application.Errors.Errors;

public class ValidationError : Error
{
    public ValidationError(Dictionary<string,string[]> dictionaryFieldReasons)
        : base("An error occurred during validation")
    {
        DictionaryFieldReasons = dictionaryFieldReasons;
    }

    public Dictionary<string,string[]> DictionaryFieldReasons { get; }
}