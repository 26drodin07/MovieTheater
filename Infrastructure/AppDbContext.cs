using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Application.Interfaces;

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

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
