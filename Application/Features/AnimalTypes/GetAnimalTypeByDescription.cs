using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Domain;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.AnimalTypes
{
    public class GetAnimalTypeByDescription
    {
        public class GetAnimalTypeByDescriptionQuery : IRequest<Result<AnimalType>>
        {
            public string Description { get; set; }
        }
        
        public class GetAnimalTypeByDescriptionHandler : IRequestHandler<GetAnimalTypeByDescriptionQuery, Result<AnimalType>>
        {
            private readonly DataContext _context;

            public GetAnimalTypeByDescriptionHandler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<Result<AnimalType>> Handle(GetAnimalTypeByDescriptionQuery request, CancellationToken cancellationToken)
            {
                var animalType = await _context.AnimalTypes.Where(x => x.Description.ToUpper() == request.Description.ToUpper())
                    .FirstOrDefaultAsync();

                if (animalType == null)
                    return Results.NotFoundError(request.Description);
                
                return animalType;
            }
        }
        
    }
}