using ApacheKafkaProducerDemo.Model;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace ApacheKafkaProducerDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private string bootstrapServers = "localhost:9092";
        private string topic = "TopicProductOrder";

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequestModel orderRequest)
        {
            string message = JsonSerializer.Serialize(orderRequest);
            bool result = await SendOrderRequest(topic, message);
            if(result)
            {
                return Ok("Message successfully sent to kafka.");
            }
            return BadRequest("Message failed to be sent");
        }


        private async Task<bool> SendOrderRequest(string topic, string message)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            try
            {

                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    var result = await producer.ProduceAsync
                    (topic, new Message<Null, string>
                    {
                        Value = message
                    });

                    Debug.WriteLine($"Delivery Timestamp:{result.Timestamp.UtcDateTime}");
                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured: {ex.Message}");
            }

            return await Task.FromResult(false);
        }
    }
}
