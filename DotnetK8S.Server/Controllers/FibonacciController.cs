using System.Threading.Tasks;
using DotnetK8S.Server.Model;
using DotnetK8S.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetK8S.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FibonacciController : ControllerBase
    {
        private readonly IFibonacciService _fibonacciService;

        public FibonacciController(IFibonacciService fibonacciService)
        {
            _fibonacciService = fibonacciService;
        }

        [HttpPost]
        public async Task Post([FromBody] CalculateFibonacciModel model)
        {
            await _fibonacciService.Calculate(model.Index);
        }
        
        [HttpGet("values")]
        public async Task<FibResultResponse> Values()
        {
            var data = await _fibonacciService.GetResults();
            return new FibResultResponse()
            {
                Data = data
            };
        }
    }
}