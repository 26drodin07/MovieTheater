using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class CinemaHallRepo : ICinemaHallRepo
    {
        private readonly AppDbContext _db;
        public CinemaHallRepo(AppDbContext db)
        {
            _db= db;
        }
        public Task<ICollection<CinemaHall>> GetHalls()
        {
            throw new NotImplementedException( );
        }
    }
}
