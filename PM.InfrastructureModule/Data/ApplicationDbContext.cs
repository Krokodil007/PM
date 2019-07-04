using Microsoft.EntityFrameworkCore;

namespace PM.InfrastructureModule.Data
{
    public class ApplicationDbContext : DbContext

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}