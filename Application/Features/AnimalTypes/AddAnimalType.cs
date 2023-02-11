using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Domain;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.AnimalTypes
{
    public class AddAnimalType
    {
        public class AddAnimalTypeCommand : IRequest<Result<AnimalType>>
        {
            public string Description { get; set; }
        }
        
        public class AddAnimalTypeValidator : AbstractValidator<AddAnimalTypeCommand>
        {
            public AddAnimalTypeValidator()
            {
                RuleFor(x => x.Description).NotEmpty();
            }
        }
        
        public class AddAnimalTypeHandler : IRequestHandler<AddAnimalTypeCommand, Result<AnimalType>>
        {
            private readonly DataContext _context;

            public AddAnimalTypeHandler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<Result<AnimalType>> Handle(AddAnimalTypeCommand request, CancellationToken cancellationToken)
            {
                var animalType = await _context.AnimalTypes
                    .Where(x => x.Description.ToUpper() == request.Description.ToUpper()).FirstOrDefaultAsync(cancellationToken);
                if (animalType != null)
                    return Results.ConflictError($"animal type {request.Description}");
                
                animalType = new AnimalType
                {
                    Description = request.Description
                };

               await _context.AnimalTypes.AddAsync(animalType,cancellationToken);
               var result = await _context.SaveChangesAsync(cancellationToken);

                if (result <= 0)
                    return Results.InternalError("Fail to add the animal type");

               return animalType;
            }
        }
    }
}