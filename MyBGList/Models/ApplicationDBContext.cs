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

            modelBuilder.Entity<BoardGames_Domains>()
                .HasKey(i => new { i.BoardGameId, i.DomainId });

            modelBuilder.Entity<BoardGames_Mechanics>()
                .HasKey(i => new { i.BoardGameId, i.MechanicId });
        }
    }
}
