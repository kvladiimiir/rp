using System;
using NATS.Client;
using System.Linq;
using System.Text;
using NATS.Client.Rx;
using NATS.Client.Rx.Ops;
using StackExchange.Redis;

namespace TextRancCalc
{
    class RedisService
    {
        public static string GetDesription(string id)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect($"localhost:6379");
            IDatabase db = redis.GetDatabase();
            string description = db.StringGet(id);

            return description;
        }
    }
    class SubscriberService
    {
        public void Run(IConnection connection)
        {
            INATSObservable<string> publish = connection.Observe("publish")
                    .Select(m => Encoding.Default.GetString(m.Data));

            publish.Subscribe(id => TextRanc.Calculate(id, RedisService.GetDesription(id)));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var subscriberService = new SubscriberService();

            using (IConnection connection = new ConnectionFactory().CreateConnection())
            {
                subscriberService.Run(connection);
                Console.WriteLine("Start:");
                Console.ReadKey();
            }
        }
    }
}
    