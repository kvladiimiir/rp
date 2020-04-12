using NATS.Client;
using System.Text;
using System.Threading.Tasks;

namespace BackendApi.Services.Publisher
{
    public class PublisherService
    {

        public PublisherService()
        {
        }

        public async Task RunAsync(IConnection connection, string id)
        {
            byte[] byteId = Encoding.Default.GetBytes(id);
            connection.Publish("publish", byteId);
            await Task.Delay(1000);
        }
    }
}