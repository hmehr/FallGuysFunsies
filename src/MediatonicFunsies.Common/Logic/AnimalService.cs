using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatonicFunsies.Common.DAL;
using MediatonicFunsies.Common.Objects;

namespace MediatonicFunsies.Common.Logic
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalsRepository _repository;

        public AnimalService(IAnimalsRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAnimal(Animal animal)
        {
            animal.Id = Guid.NewGuid();
            animal.CreationDate = DateTime.Now;

            await _repository.AddAnimal(animal);

        }

        public async Task<Animal> GetAnimal(Guid id)
        {
            Animal animal = await _repository.GetAnimalById(id);
            if (animal == null)
            {
                throw new ArgumentException($"Invalid animalId {id}", nameof(id));
            }

            IEnumerable<Metric> metrics = await _repository.GetAllAnimalMetrics(id);
            animal.Metrics = metrics;

            return animal;
        }

        public Task<IEnumerable<Animal>> GetAnimalByOwnerId(Guid ownerId)
        {
            return _repository.GetAllAnimalsByOwnerId(ownerId);
        }

        public async Task AddMetric(Guid animalId, Metric metric)
        {
            Animal animal = await GetAnimal(animalId);
            if (animal == null)
            {
                throw new ArgumentException($"Invalid animalId {animalId}", nameof(animalId));
            }
            metric.Id = Guid.NewGuid();
            metric.CreationDate = DateTime.Now;
            await _repository.AddMetric(animalId, metric);
        }

        public async Task AddMetricModifier(Guid animalId, Guid metricId, MetricModifier modifier)
        {
            Animal animal = await GetAnimal(animalId);
            if (animal == null)
            {
                throw new ArgumentException($"Invalid animalId {animalId}", nameof(animalId));
            }

            Metric metric = animal.Metrics.FirstOrDefault(m => m.Id == metricId);

            if (metric == null)
            {
                throw new ArgumentException($"Invalid metricId {metricId}", nameof(metricId));
            }

            if (modifier.ActionDate <= metric.CreationDate)
            {
                throw new ArgumentException($"Invalid date for modifier's action date {modifier.ActionDate}. It can't be before the creation of the metric {metric.CreationDate}", nameof(modifier.ActionDate));
            }

            if (metric.Rate * modifier.ModificationEffect > 0)
            {
                string sign = metric.Rate > 0 ? "negative" : "positive";
                throw new ArgumentException($"Modifier is invalid. Modifier has to be {sign}");
            }

            await _repository.AddMetricModifier(animalId, metricId, modifier);

        }
    }
}