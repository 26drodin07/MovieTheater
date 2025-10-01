using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class MovieRepo : IMovieRepo
    {
        private readonly AppDbContext _db;
        public MovieRepo(AppDbContext db)
        {
            _db = db;
        }
        public Task<ICollection<Movie>> GetMovies()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Movie>> SearchMovieByName(string name, MovieFiltersDTO filters)
        {
            throw new NotImplementedException();
        }
    }
}
