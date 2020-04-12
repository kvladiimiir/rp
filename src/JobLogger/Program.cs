using System;
using NATS.Client;
using System.Linq;
using System.Text;
using NATS.Client.Rx;
using NATS.Client.Rx.Ops;

namespace JobLogger
{
    class SubscriberService
    {
        public void Run(IConnection connection)
        {
            INATSObservable<string> publish = connection.Observe("publish")
                    .Select(m => Encoding.Default.GetString(m.Data));

            publish.Subscribe(id => Console.WriteLine(id + ", " + RedisDbContext.GetDesription(id)));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var subscriberService = new SubscriberService();

            using (IConnection connection = new ConnectionFactory().CreateConnection())
            {
                subscriberService.Run(connection);
                Console.WriteLine("Monitoring starting:");
                Console.ReadKey();
            }
        }
    }
}
    