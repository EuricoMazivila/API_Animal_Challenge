using Application.Errors;
using Application.Extensions;
using Application.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public class ExceptionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : ResultBase, new()
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IExceptionHandlingStrategy<TRequest, TResponse>> _exceptionHandlingStrategies;
    private readonly ILogger<ExceptionPipelineBehavior<TRequest, TResponse>> _logger;

    public ExceptionPipelineBehavior(
        IEnumerable<IExceptionHandlingStrategy<TRequest, TResponse>> exceptionHandlingStrategies,
        ILogger<ExceptionPipelineBehavior<TRequest,TResponse>> logger)
    {
        _exceptionHandlingStrategies = exceptionHandlingStrategies;
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, 
        RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception exception)
        {
            var strategies =
                _exceptionHandlingStrategies
                    .FirstOrDefault(strategies => 
                        strategies.MustExecute(exception, request));

            if (strategies is null)
            {
                _logger.LogError(exception, "Unhandled error");
                return Results.InternalError().To<TResponse>();
            }

            var result = await strategies.Execute(exception, request, cancellationToken) ??
                         Results.InternalError().To<TResponse>();

            return result;
        }
    }
}