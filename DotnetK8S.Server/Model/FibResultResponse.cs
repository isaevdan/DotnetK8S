using DotnetK8S.DAL;

namespace DotnetK8S.Server.Model
{
    public class FibResultResponse
    {
        public FibResult[] Data { get; set; }
    }
}