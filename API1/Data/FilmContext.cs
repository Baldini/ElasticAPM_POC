using Microsoft.EntityFrameworkCore;

namespace API1.Data
{
    public class FilmContext : DbContext
    {
        public DbSet<Film> Films { get; set; }

        public FilmContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Films");

                entity.Property(e => e.Id).IsRequired().UseIdentityColumn();
            });
        }
    }
}
