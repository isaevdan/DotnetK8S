using Microsoft.EntityFrameworkCore;

namespace DotnetK8S.DAL
{
    public class FibonacciContext : DbContext
    {
        public FibonacciContext(DbContextOptions<FibonacciContext> options) : base(options)
        {
            
        }
        
        public DbSet<FibResult> FibResults { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<FibResult>();
            entity.ToTable("FibResult");
            
        }
    }
}