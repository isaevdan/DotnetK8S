using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotnetK8S.Worker
{
    public class WorkerService : BackgroundService
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly Config _config;
        private readonly IFibResultRepository _repository;

        public WorkerService(IOptions<Config> config, ILogger<WorkerService> logger, IFibResultRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _config = config.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            _logger.LogInformation("Start WorkerService");
            var config = new ConsumerConfig
            {
                BootstrapServers = _config.Kafka.BootstrapServers,
                GroupId = _config.Kafka.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            
            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(_config.Kafka.Topic);

                while (!token.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(token);

                    if (int.TryParse(consumeResult.Message.Value, out var number))
                    {
                        var result = FibonacciCalculator.Calculate(number);
                        await _repository.Save(new FibResult(number, result));
                        _logger.LogInformation("No: {No}, Result: {Result}", number, result);
                    }
                    else
                    {
                        _logger.LogError("Error parsing value: {Value}", consumeResult.Message.Value);
                    }
                }

                consumer.Close();
            }
            _logger.LogInformation("Stop WorkerService");
        }
    }
}