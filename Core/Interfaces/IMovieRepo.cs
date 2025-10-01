using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IMovieRepo
    {
        Task<ICollection<Movie>> GetMovies();
        Task<ICollection<Movie>> SearchMovieByName(string name, MovieFiltersDTO filters);
       

    }
}
