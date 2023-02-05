using API.Serialization;
using Application.Features.AnimalTypes;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AnimalTypeController : BaseController
    {
        private readonly ISender _sender;

        public AnimalTypeController(ISender sender)
        {
            _sender = sender;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ListAnimalType(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new ListAnimalType.ListAnimalTypeQuery(), cancellationToken);
            return this.SerializeResult(result);
        }
        
        [HttpGet("{description}")]
        public async Task<ActionResult<AnimalType>> GetAnimalTypeByDescription(string description)
        {
            return await Mediator.Send(new GetAnimalTypeByDescription.GetAnimalTypeByDescriptionQuery
                {Description = description});
        }

        [HttpPost]
        public async Task<ActionResult<AnimalType>> AddAnimalType(AddAnimalType.AddAnimalTypeCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AnimalType>> UpdateAnimalType(int id, UpdateAnimalType.UpdateAnimalTypeCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<AnimalType>> RemoveAnimalType(int id)
        {
            return await Mediator.Send(new DeleteAnimalType.DeleteAnimalTypeCommand {Id = id});
        } 
    }
}