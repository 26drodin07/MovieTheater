using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        DbSet<CinemaHall> CinemaHalls { get; set; }
        DbSet<Movie> Movies { get; set; }
        DbSet<MovieGenre> Genres { get; set; }
        DbSet<MovieSession> Sessions { get; set; }
        DbSet<PricePolicy> PricePolicies { get; set; }
        DbSet<SalePolicy> SalePolicies { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
        {
          
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }
    }
}
