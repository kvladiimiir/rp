using StackExchange.Redis;

namespace JobLogger
{
    class RedisDbContext
    {
        public static string GetDesription(string id)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect($"localhost:6379");
            IDatabase db = redis.GetDatabase();
            string description = db.StringGet("ds" + id);

            return description;
        }
    }
}