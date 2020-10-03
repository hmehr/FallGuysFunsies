using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatonicFunsies.Common.Objects;

namespace MediatonicFunsies.Common.Logic
{
    public interface IAnimalService
    {
        Task AddAnimal(Animal animal);
        Task DeleteAnimal(Guid animalId);
        Task<Animal> GetAnimal(Guid id);
        Task<IEnumerable<Animal>> GetAnimalByOwnerId(Guid ownerId);
        Task AddMetric(Guid animalId, Metric metric);
        Task AddMetricModifier(Guid animalId, Guid metricId, MetricModifier modifier);
    }
}