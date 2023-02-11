using Application.Errors;
using Domain;
using FluentResults;
using MediatR;
using Persistence;

namespace Application.Features.AnimalTypes
{
    public class DeleteAnimalType
    {
        public class DeleteAnimalTypeCommand : IRequest<Result<AnimalType>>
        {
            public int Id { get; set; }
        }
        
        public class DeleteAnimalTypeHandler : IRequestHandler<DeleteAnimalTypeCommand, Result<AnimalType>>
        {
            private readonly DataContext _context;

            public DeleteAnimalTypeHandler(DataContext context)
            {
                _context = context;
            }
            public async Task<Result<AnimalType>> Handle(DeleteAnimalTypeCommand request, CancellationToken cancellationToken)
            {
                var animalType = await _context.AnimalTypes.FindAsync(request.Id);
                if (animalType is null)
                    return Results.NotFoundError($"Id {request.Id} of animal type");

                _context.AnimalTypes.Remove(animalType);
                var result = await _context.SaveChangesAsync(cancellationToken);

                if (result <= 0)
                    return Results.InternalError("Fail to delete the animal type");

                return animalType;
            }
        }
    }
}