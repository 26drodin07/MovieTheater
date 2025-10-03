using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<CinemaHall> CinemaHalls { get; set; }
        DbSet<MovieGenre> Genres { get; set; }
        DbSet<Movie> Movies { get; set; }
        DbSet<PricePolicy> PricePolicies { get; set; }
        DbSet<SalePolicy> SalePolicies { get; set; }
        DbSet<MovieSession> Sessions { get; set; }
    }
}
