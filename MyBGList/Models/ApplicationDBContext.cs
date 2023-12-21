using Microsoft.EntityFrameworkCore;

namespace MyBGList.Models
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(
            DbContextOptions<ApplicationDBContext> options) 
            : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
