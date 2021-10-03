using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DotnetK8S.Server.Services
{
    public interface IFibonacciService
    {
        Task Calculate(int no);
    }

    public class FibonacciService : IFibonacciService
    {
        private readonly IKafkaService _kafkaService;
        private readonly FibonacciConfig _config;

        public FibonacciService(IKafkaService kafkaService, IOptions<FibonacciConfig> config)
        {
            _kafkaService = kafkaService;
            _config = config.Value;
        }

        public async Task Calculate(int no)
        {
            await _kafkaService.Post(no.ToString(), no.ToString(), _config.KafkaTopic);
        }
    }
}