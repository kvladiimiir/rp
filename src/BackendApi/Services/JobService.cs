using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using NATS.Client;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Threading;
using System.IO;

namespace BackendApi.Services
{
    public class JobService : Job.JobBase
    {
        public override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            ConnectionMultiplexer conn = ConnectionMultiplexer.Connect($"localhost:6379");
            IDatabase db = conn.GetDatabase();
            
            string id = Guid.NewGuid().ToString();
            db.StringSet(id, request.Data);
            var publisherService = new Publisher.PublisherService();
            
            using (IConnection connection = new ConnectionFactory().CreateConnection($"localhost:4222"))
            {
                publisherService.RunAsync(connection, id).Wait();
            }
            
            db.StringSet("ds" + id, request.Description);
            var responce = new RegisterResponse
            {
                Id = id
            };
            
            return Task.FromResult(responce);
        }

        public override Task<GetProcessingResultResponce> GetProcessingResult(GetProcessingResultRequest request, ServerCallContext context)
        {
            string id = request.Id;

            string status = "inProgress";
            string resultRanc = String.Empty;

            var responce = new GetProcessingResultResponce { Ranc = resultRanc, Status = status };
            var conn = ConnectionMultiplexer.Connect($"localhost:6379");
            IDatabase db = conn.GetDatabase();

            for (int i = 0; i < 10; i++)
            {
                resultRanc = db.StringGet("resultRanc" + id);

                if (!resultRanc.Equals(String.Empty))
                {
                    responce.Status = "Completed";
                    responce.Ranc = resultRanc;
                    break;
                }

                Thread.Sleep(1000);
            }

            return Task.FromResult(responce);
        }

    }
}