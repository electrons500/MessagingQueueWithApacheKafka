using ApacheKafkaConsumerDemo.Model;
using Confluent.Kafka;
using System.Diagnostics;
using System.Text.Json;

namespace ApacheKafkaConsumerDemo.Service
{
    public class ApacheKafkaConsumerService:IHostedService
    {
        private string topic = "TopicProductOrder";
        private string groupId = "ProductOrdering";
        private string bootstrapServers = "localhost:9092";

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            try
            {
                using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumerBuilder.Subscribe(topic);
                    var cancelToken = new CancellationTokenSource();

                    try
                    {
                        while (true)
                        {
                            var consumer = consumerBuilder.Consume
                               (cancelToken.Token);
                            var orderRequest = JsonSerializer.Deserialize<OrderRequestModel>(consumer.Message.Value);
                            Debug.WriteLine($"order id:{orderRequest.OrderId}, product id:{orderRequest.ProductId}, customer id:{orderRequest.CustomerId}, Quantity: {orderRequest.Quantity}, status: {orderRequest.Status}");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumerBuilder.Close();
                    }
                }
            }
            catch (Exception ex)
            {
               Debug.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
