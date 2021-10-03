using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using DotnetK8S.DAL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotnetK8S.Worker
{
    public class WorkerService : BackgroundService
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly Config _config;
        private readonly IFibResultRepository _repository;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public WorkerService(
            IOptions<Config> config, 
            ILogger<WorkerService> logger, 
            IFibResultRepository repository,
            IHostApplicationLifetime applicationLifetime)
        {
            _applicationLifetime = applicationLifetime;
            _logger = logger;
            _repository = repository;
            _config = config.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            try
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
                        _logger.LogInformation("Start iteration");

                        var consumeResult = consumer.Consume(token);
                        _logger.LogInformation("Value consumed");

                        if (int.TryParse(consumeResult.Message.Value, out var number))
                        {
                            _logger.LogInformation("Start calculation for: {No}", number);
                            var result = FibonacciCalculator.Calculate(number);
                            _logger.LogInformation("Inserting No: {No}, Result: {Result}", number, result);

                            await _repository.Save(new FibResult(number, result));
                            _logger.LogInformation("No: {No}, Result: {Result}", number, result);
                        }
                        else
                        {
                            _logger.LogError("Error parsing value: {Value}", consumeResult.Message.Value);
                        }

                        _logger.LogInformation("Finish iteration");
                    }

                    consumer.Close();
                }

                _logger.LogInformation("Stop WorkerService");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Worker service error");
                _applicationLifetime.StopApplication();
            }
        }
    }
}