using Domain;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.AnimalTypes
{
    public class ListAnimalType
    {
        public class ListAnimalTypeQuery : IRequest<Result<IReadOnlyList<AnimalType>>>
        {
        }
        
        public class ListAnimalTypeHandler: IRequestHandler<ListAnimalTypeQuery, Result<IReadOnlyList<AnimalType>>>
        {
            private readonly DataContext _context;

            public ListAnimalTypeHandler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<Result<IReadOnlyList<AnimalType>>> Handle(ListAnimalTypeQuery request, CancellationToken cancellationToken)
            {
                var result = await _context.AnimalTypes.ToListAsync(cancellationToken);
                return result;
            }
        }
    }
}