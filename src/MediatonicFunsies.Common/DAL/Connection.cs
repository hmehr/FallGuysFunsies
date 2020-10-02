using System;
using MediatonicFunsies.Common.Objects;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace MediatonicFunsies.Common.DAL
{
    public class Connection : IConnection
    {
        /// <summary>
        /// The _connection.
        /// </summary>
        private readonly Lazy<IConnectionMultiplexer> _connection;


        public Connection(IOptions<RedisConfiguration> redis)
        {
            _connection = new Lazy<IConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redis.Value.ConnectionString));
        }

        public IDatabase GetDatabase()
        {
            return _connection.Value.GetDatabase();
        }
    }
}