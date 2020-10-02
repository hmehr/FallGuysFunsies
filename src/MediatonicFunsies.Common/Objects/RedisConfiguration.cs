using System;

namespace MediatonicFunsies.Common.Objects
{
    public class RedisConfiguration
    {
        public string ConnectionString { get; set; } = Environment.GetEnvironmentVariable("RedisConnectionString");
        
    }
}