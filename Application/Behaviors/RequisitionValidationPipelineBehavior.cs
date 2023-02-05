using Application.Errors;
using Application.Extensions;
using FluentResults;
using FluentValidation;
using MediatR;

namespace Application.Behaviors;

public class RequisitionValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : ResultBase, new()
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequisitionValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, 
        RequestHandlerDelegate<TResponse> next)
    {
        if (!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResult = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var dictionaryFieldReasons = validationResult
            .SelectMany(result => result.Errors)
            .GroupBy(error => error.PropertyName)
            .ToDictionary(errorGroup => 
                errorGroup.Key, errorGroup => 
                errorGroup.Select(e => e.ErrorMessage).ToArray());
        

        if (dictionaryFieldReasons.Count != 0)
            return Results.ValidationError(dictionaryFieldReasons).To<TResponse>();

        return await next();
    }
}