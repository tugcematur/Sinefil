using Microsoft.EntityFrameworkCore;
using Sinefil.Models.Data.Class;
using System.Xml.Linq;

namespace Sinefil.Models.Data
{
    public class SinefilContext : DbContext
    {
        public SinefilContext(DbContextOptions<SinefilContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Film)
                .WithMany(f => f.Reviews)
                .HasForeignKey(r => r.FilmId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Film)
                .WithMany(f => f.Reviews)
                .HasForeignKey(r => r.FilmId)
                .OnDelete(DeleteBehavior.Cascade);


        }


        public DbSet<User> User { get; set; }
        public DbSet<Film> Film { get; set; }
        public DbSet<Review> Review { get; set; }

  
    }
}
