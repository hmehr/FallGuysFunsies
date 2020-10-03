using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatonicFunsies.Common.Logic;
using MediatonicFunsies.Common.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MediatonicFunsies.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("v{version:apiVersion}/funsies")]
    public class FunsiesController : ControllerBase
    {
       

        private readonly ILogger<FunsiesController> _logger;
        private readonly IAnimalService _animalService;

        public FunsiesController(ILogger<FunsiesController> logger, IAnimalService animalService)
        {
            _logger = logger;
            _animalService = animalService;
        }

        [HttpGet]
        public IEnumerable<int> Get()
        {
            return new[] {1, 2};
        }

        [HttpPost("animals")]
        public async Task AddAnimal([FromBody] Animal animal)
        {
            await _animalService.AddAnimal(animal);
        }

        [HttpGet("animals/{id}")]
        public async Task<Animal> GetAnimal([FromRoute] Guid id)
        {
            return await _animalService.GetAnimal(id);
        }

        [HttpDelete("animals/{id}")]
        public async Task DeleteAnimal([FromRoute] Guid id)
        {
            await _animalService.DeleteAnimal(id);
        }

        [HttpGet("person/{ownerId}/animals")]
        public async Task<IEnumerable<Animal>> GetAnimalByOwnerId([FromRoute] Guid ownerId)
        {
            return await _animalService.GetAnimalByOwnerId(ownerId);
        }

        [HttpPost("animals/{animalId}/metric")]
        public async Task AddMetric([FromBody] Metric metric, [FromRoute] Guid animalId)
        {
            await _animalService.AddMetric(animalId, metric);
        }

        [HttpDelete("animals/{animalId}/metric/{metricId}")]
        public async Task DeleteMetric([FromRoute] Guid metricId, [FromRoute] Guid animalId)
        {
            await _animalService.DeleteMetric(animalId, metricId);
        }

        [HttpPost("animals/{animalId}/metric/{metricId}")]
        public async Task AddMetricModifier([FromRoute] Guid metricId, [FromRoute] Guid animalId, [FromBody] MetricModifier modifier)
        {
            await _animalService.AddMetricModifier(animalId, metricId, modifier);
        }
    }
}
