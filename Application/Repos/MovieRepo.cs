using Application.Extensions;
using Application.Interfaces;
using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
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

        

        public async Task<ICollection<MovieGetDTO>> GetMovies()
        {
            return await _db.Movies.Select(x=>x.ToGetDTO()).ToListAsync();
        }

        public async Task<ICollection<MovieGetDTO>> GetFiltered(string name, MovieFiltersDTO filters)
        {
            throw new NotImplementedException();
        }
    }
}
