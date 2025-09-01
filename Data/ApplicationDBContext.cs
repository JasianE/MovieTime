using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<UserMovie> UserMovies { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserMovie>(entity => entity.HasKey(userMovie => new { userMovie.AppUserId, userMovie.MovieId }));

            builder.Entity<UserMovie>()
            .HasOne(userMovie => userMovie.AppUser)
            .WithMany(userMovie => userMovie.UserMovies)
            .HasForeignKey(userMovie => userMovie.AppUserId);

            builder.Entity<UserMovie>()
            .HasOne(userMovie => userMovie.Movie)
            .WithMany(userMovie => userMovie.UserMovies)
            .HasForeignKey(userMovie => userMovie.MovieId);

            builder.Entity<Movie>()
            .HasIndex(m => m.Title)
            .IsUnique(); // ensure that its unique

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Id = "1"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                    Id = "2"
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}