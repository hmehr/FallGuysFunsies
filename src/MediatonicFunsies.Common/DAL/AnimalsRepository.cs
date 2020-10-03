using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MediatonicFunsies.Common.Objects;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace MediatonicFunsies.Common.DAL
{
    public class AnimalsRepository : IAnimalsRepository
    {
        private readonly IConnection _connection;
        private readonly ILogger<AnimalsRepository> _logger;

        public AnimalsRepository(IConnection connection, ILogger<AnimalsRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<Animal> GetAnimalById(Guid id)
        {
            RedisValue animalJson = await _connection.GetDatabase().StringGetAsync($"Animals:{id}");
            return JsonSerializer.Deserialize<Animal>(animalJson.ToString());
        }

        public async Task<IEnumerable<Animal>> GetAllAnimalsByOwnerId(Guid ownerId)
        {
            //get the ids from ownership
            RedisValue[] animalIds = await _connection.GetDatabase().ListRangeAsync($"AnimalOwnership:{ownerId}");
            _logger.LogDebug($"Animal ids found: {string.Join(',',animalIds)}");

            //convert to proper keys
            RedisKey[] arr = Array.ConvertAll(animalIds, x => new RedisKey($"Animals:{x}"));

            //get all the animal objects by their ids
            RedisValue[] animals =await _connection.GetDatabase().StringGetAsync(arr);

            _logger.LogDebug($"Animals found: {animals?.Count() ?? 0}");

            //deserialize
            return animals?.Select(a => JsonSerializer.Deserialize<Animal>(a)).ToList();
        }

        public async Task<IEnumerable<Metric>> GetAllAnimalMetrics(Guid animalId)
        {
            HashEntry[] metrics = await _connection.GetDatabase().HashGetAllAsync($"Metrics:{animalId}");
            return metrics.Select(m => JsonSerializer.Deserialize<Metric>(m.Value));
        }

        public async Task AddAnimal(Animal animal)
        {
            //We can use a transaction here
            await _connection.GetDatabase().StringSetAsync($"Animals:{animal.Id}", JsonSerializer.Serialize(animal));
            await _connection.GetDatabase().ListRightPushAsync($"AnimalOwnership:{animal.Owner}",$"{animal.Id}");
        }

        public async Task DeleteAnimal(Guid ownerId, Guid animalId)
        {
            await _connection.GetDatabase().ListRemoveAsync($"AnimalOwnership:{ownerId}", $"{animalId}");
            await _connection.GetDatabase().KeyDeleteAsync($"Animals:{animalId}");
        }

        public async Task AddMetric(Guid animalId, Metric metric)
        {
            await _connection.GetDatabase().HashSetAsync($"Metrics:{animalId}", metric.Id.ToString(), JsonSerializer.Serialize(metric));
        }

        public async Task<Metric> GetMetric(Guid animalId, Guid metricId)
        {
            RedisValue metric = await _connection.GetDatabase().HashGetAsync($"Metrics:{animalId}", metricId.ToString());
            return metric.HasValue ? JsonSerializer.Deserialize<Metric>(metric) : null;
        }

        public async Task AddMetricModifier(Guid animalId,Guid metricId, MetricModifier modifier)
        {
            Metric metric = await GetMetric(animalId, metricId);
            if (metric.Modifiers == null)
            {
                metric.Modifiers = new List<MetricModifier>();
            }
            List<MetricModifier> modifiers =  metric.Modifiers.ToList();
            modifiers.Add(modifier);
            metric.Modifiers = modifiers;

            await _connection.GetDatabase().HashSetAsync($"Metrics:{animalId}", metricId.ToString(),
                JsonSerializer.Serialize(metric));
        }

        public async Task UpdateMetric(Guid animalId, Metric metric)
        {
            Metric metricToUpdate = await GetMetric(animalId, metric.Id);
            metric.Modifiers = metricToUpdate.Modifiers;

            await _connection.GetDatabase().HashSetAsync($"Metrics:{animalId}", metric.Id.ToString(), JsonSerializer.Serialize(metric));
        }
    }
}