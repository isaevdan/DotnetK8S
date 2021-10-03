namespace DotnetK8S.Server
{
    public class Config
    {
        public KafkaConfig Kafka { get; set; }
        public FibonacciConfig Fibonacci { get; set; }
    }

    public class FibonacciConfig
    {
        public string KafkaTopic { get; set; }
    }
}