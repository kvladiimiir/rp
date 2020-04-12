using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using NATS.Client;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace BackendApi.Services
{
    public class JobService : Job.JobBase
    {
        public void ConnectToRedis(string id, string ds)
        {
            var conn = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = conn.GetDatabase();
            db.StringSet(id, ds);
        }

        public override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            string id = Guid.NewGuid().ToString();
            ConnectToRedis(id, request.Description);

            var publisherService = new Publisher.PublisherService();
            
            using (IConnection connection = new ConnectionFactory().CreateConnection("localhost:4222"))
            {
                publisherService.RunAsync(connection, id).Wait();
            }

            var responce = new RegisterResponse
            {
                Id = id
            };
            
            return Task.FromResult(responce);
        }
    }
}