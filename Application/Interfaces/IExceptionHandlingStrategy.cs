using FluentResults;
using MediatR;

namespace Application.Interfaces;

public interface IExceptionHandlingStrategy<TRequest, TResponse>
    where TResponse : ResultBase, new()
    where TRequest : IRequest<TResponse>
{
    bool MustExecute(Exception exception, TRequest request);
    Task<TResponse?> Execute(Exception exception, TRequest request, CancellationToken cancellationToken = default);
}