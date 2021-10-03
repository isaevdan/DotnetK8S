using System.Net;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace DotnetK8S.Server.Services
{
    public interface IKafkaService
    {
        Task Post<TKey, TValue>(TKey key, TValue value, string topic);
    }

    public class KafkaService : IKafkaService
    {
        private readonly KafkaConfig _config;

        public KafkaService(IOptions<KafkaConfig> config)
        {
            _config = config.Value;
        }

        public async Task Post<TKey, TValue>(TKey key, TValue value, string topic)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _config.BootstrapServers,
                ClientId = Dns.GetHostName()
            };

            using var producer = new ProducerBuilder<TKey, TValue>(config).Build();
            await producer.ProduceAsync(topic, new Message<TKey, TValue> { Key = key, Value = value});
        }
    }
}