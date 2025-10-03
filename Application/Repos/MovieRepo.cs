using Application.Interfaces;
using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repos
{
    public class MovieRepo : IMovieRepo
    {
        private readonly IAppDbContext _db;
        public MovieRepo(IAppDbContext db)
        {
            _db = db;
        }

        

        public async Task<ICollection<Movie>> GetMovies()
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Movie>> SearchMovieByName(string name, MovieFiltersDTO filters)
        {
            throw new NotImplementedException();
        }
    }
}
