using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Features.AnimalTypes
{
    public class ListAnimalType
    {
        public class ListAnimalTypeQuery : IRequest<IReadOnlyList<AnimalType>>
        {
        }
        
        public class ListAnimalTypeHandler: IRequestHandler<ListAnimalTypeQuery, IReadOnlyList<AnimalType>>
        {
            private readonly DataContext _context;
            private readonly ILogger<ListAnimalTypeHandler> _logger;

            public ListAnimalTypeHandler(DataContext context, ILogger<ListAnimalTypeHandler> logger)
            {
                _context = context;
                _logger = logger;
            }
            
            public async Task<IReadOnlyList<AnimalType>> Handle(ListAnimalTypeQuery request, CancellationToken cancellationToken)
            {
                _logger.LogWarning("Example");
                _logger.LogError("Example");
                return await _context.AnimalTypes.ToListAsync();
            }
        }
    }
}