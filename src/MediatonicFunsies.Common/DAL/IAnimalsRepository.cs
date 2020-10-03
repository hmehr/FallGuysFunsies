using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatonicFunsies.Common.Objects;


namespace MediatonicFunsies.Common.DAL
{
    public interface IAnimalsRepository
    {
        Task<Animal> GetAnimalById(Guid id);

        Task<IEnumerable<Animal>> GetAllAnimalsByOwnerId(Guid ownerId);

        Task<IEnumerable<Metric>> GetAllAnimalMetrics(Guid animalId);

        Task AddAnimal(Animal animal);

        Task DeleteAnimal(Guid ownerId, Guid animalId);

        Task AddMetric(Guid animalId, Metric metric);

        Task DeleteMetric(Guid animalId, Guid metricId);

        Task AddMetricModifier(Guid animalId, Guid metricId, MetricModifier modifier);

        Task UpdateMetric(Guid animalId, Metric metric);

    }
}