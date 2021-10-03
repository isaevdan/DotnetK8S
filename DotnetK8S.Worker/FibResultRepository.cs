using System.Threading.Tasks;

namespace DotnetK8S.Worker
{
    public interface IFibResultRepository
    {
        Task Save(FibResult result);
    }

    public class FibResultRepository : IFibResultRepository
    {
        public Task Save(FibResult result)
        {
            return Task.CompletedTask;
        }
    }

    public class FibResult
    {
        public FibResult()
        {
            
        }
        public FibResult(int number, int result)
        {
            Number = number;
            Result = result;
        }

        public int Number { get; set; }
        public int Result { get; set; }
    }
}