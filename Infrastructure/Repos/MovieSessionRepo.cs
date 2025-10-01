using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class MovieSessionRepo : IMovieSessionRepo
    {
        private readonly AppDbContext _db;
        public MovieSessionRepo(AppDbContext db)
        {
            _db = db;
        }
        public Task<ICollection<MovieSession>> GetSessions()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<MovieSession>> GetSessionsByMovie(int movieId)
        {
            throw new NotImplementedException();
        }
    }
}
