using API.Serialization;
using Application.Features.AnimalTypes;
using Domain;
using MediatR;
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
        public async Task<IActionResult> ListAnimalType(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new ListAnimalType.ListAnimalTypeQuery(), cancellationToken);
            return this.SerializeResult(result);
        }
        
        [HttpGet("{description}")]
        public async Task<IActionResult> GetAnimalTypeByDescription(string description)
        {
            var result = await _sender.Send(new GetAnimalTypeByDescription.GetAnimalTypeByDescriptionQuery
            { Description = description });
            return this.SerializeResult(result);    
        }

        [HttpPost]
        public async Task<IActionResult> AddAnimalType(AddAnimalType.AddAnimalTypeCommand command)
        {
            var result = await _sender.Send(command);
            return this.SerializeResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnimalType(int id, UpdateAnimalType.UpdateAnimalTypeCommand command)
        {
            command.Id = id;
            var result = await _sender.Send(command);
            return this.SerializeResult(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAnimalType(int id)
        {
            var result = await _sender.Send(new DeleteAnimalType.DeleteAnimalTypeCommand { Id = id });
            return this.SerializeResult(result);
        } 
    }
}