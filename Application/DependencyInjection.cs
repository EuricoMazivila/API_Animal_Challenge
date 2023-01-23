using Application.Features.AnimalTypes;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddControllers()
            .AddFluentValidation(s =>
            {
                s.RegisterValidatorsFromAssemblyContaining<ListAnimalType.ListAnimalTypeQuery>();
                s.DisableDataAnnotationsValidation = true;
            });
        services.AddMediatR(typeof(ListAnimalType.ListAnimalTypeQuery).Assembly);
        return services;
    }
}