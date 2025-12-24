using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Use LocalDb connection string (same as in appsettings.json)
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StationDb;Trusted_Connection=true;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
