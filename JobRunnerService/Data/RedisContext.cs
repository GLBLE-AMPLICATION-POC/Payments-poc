using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

namespace JobRunnerService.Data
{
    public class RedisContext
    {
        private readonly ConnectionMultiplexer _redis;
        public IDatabase Database { get; }

        public RedisContext(IConfiguration configuration)
        {
            var connectionString = configuration["Redis:ConnectionString"] ?? throw new ArgumentNullException("Redis:ConnectionString");
            _redis = ConnectionMultiplexer.Connect(connectionString);
            Database = _redis.GetDatabase();
        }
    }
}