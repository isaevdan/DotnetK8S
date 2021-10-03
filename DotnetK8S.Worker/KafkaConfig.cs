namespace DotnetK8S.Worker
{
    public class KafkaConfig
    {
        public string BootstrapServers { get; set; }
        public string ConsumerGroupId { get; set; }
        public string Topic { get; set; }
    }
}