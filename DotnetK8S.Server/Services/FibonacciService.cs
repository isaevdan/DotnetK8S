using System.Threading.Tasks;
using DotnetK8S.DAL;
using Microsoft.Extensions.Options;

namespace DotnetK8S.Server.Services
{
    public interface IFibonacciService
    {
        Task<FibResult[]> GetResults();
        Task Calculate(int no);
    }

    public class FibonacciService : IFibonacciService
    {
        private readonly IKafkaService _kafkaService;
        private readonly FibonacciConfig _config;
        private readonly IFibResultRepository _fibResultRepository;

        public FibonacciService(IKafkaService kafkaService, IOptions<FibonacciConfig> config, IFibResultRepository fibResultRepository)
        {
            _kafkaService = kafkaService;
            _fibResultRepository = fibResultRepository;
            _config = config.Value;
        }

        public async Task Calculate(int no)
        {
            await _kafkaService.Post(no.ToString(), no.ToString(), _config.KafkaTopic);
        }

        public async Task<FibResult[]> GetResults()
        {
            return await _fibResultRepository.GetAll();
        }
    }
}