using Application.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    

    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<CinemaHall> CinemaHalls { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieGenre> Genres { get; set; }
        public DbSet<MovieSession> Sessions { get; set; }
        public DbSet<PricePolicy> PricePolicies { get; set; }
        public DbSet<SalePolicy> SalePolicies { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
        {
            Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
         
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(m => 
            {
                m.HasMany(x => x.Sessions).WithOne(x => x.Movie).OnDelete(DeleteBehavior.Cascade);
                m.HasMany(x => x.MovieGenres).WithMany(x => x.Movies);
            });
            modelBuilder.Entity<CinemaHall>(ch =>
            {
                ch.HasMany(x=>x.MovieSessions).WithOne(x=>x.CinemaHall).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<MovieSession>(ms => 
            {
                ms.HasMany(x => x.PricePolices).WithOne(x => x.MovieSession).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<MovieGenre>();
            modelBuilder.Entity<PricePolicy>();
            modelBuilder.Entity<SalePolicy>();
        }
    }
}
