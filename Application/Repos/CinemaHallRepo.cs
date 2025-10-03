using Application.Interfaces;
using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repos
{
    public class CinemaHallRepo : ICinemaHallRepo
    {
        private readonly IAppDbContext _db;
        public CinemaHallRepo(IAppDbContext db)
        {
            _db= db;
        }
        public Task<ICollection<CinemaHall>> GetHalls()
        {
            throw new NotImplementedException( );
        }
    }
}
