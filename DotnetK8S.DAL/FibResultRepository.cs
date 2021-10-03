using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotnetK8S.DAL
{
    public interface IFibResultRepository
    {
        Task<FibResult[]> GetAll();
        Task Save(FibResult result);
    }

    public class FibResultRepository : IFibResultRepository
    {
        private readonly IDbContextFactory<FibonacciContext> _dbContextFactory;

        public FibResultRepository(IDbContextFactory<FibonacciContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task Save(FibResult result)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();
            var prev = await dbContext.FibResults.FindAsync(result.Number);
            if (prev == null)
            {
                await dbContext.FibResults.AddAsync(result);
                await dbContext.SaveChangesAsync();
            }
        }
        
        public async Task<FibResult[]> GetAll()
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();
            return await dbContext.FibResults.ToArrayAsync();
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

        [Key]
        public int Number { get; set; }
        public int Result { get; set; }
    }
}