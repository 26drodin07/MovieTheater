using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IMovieRepo Movies { get; set; } 
        public IMovieSessionRepo Sessions { get; set; }
        public ICinemaHallRepo CinemaHalls { get; set; }

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Movies = new MovieRepo(db);
            Sessions = new MovieSessionRepo(db);
            CinemaHalls = new CinemaHallRepo(db);
        }
    }
}
