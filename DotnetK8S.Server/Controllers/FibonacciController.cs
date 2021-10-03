using System.Threading.Tasks;
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
        public async Task Post([FromBody] int no)
        {
            await _fibonacciService.Calculate(no);
        }
    }
}