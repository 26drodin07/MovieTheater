using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    internal interface IMovieRepo
    {
        ICollection<Movie> GetMovies();
        ICollection<Movie> SearchMovieByName(string name, MovieFiltersDTO filters);
        ICollection<Movie> GetMoviesByReleaseDate(DateTime date);

        ICollection<CinemaHall> GetMovieHalls();
        ICollection<MovieSession> GetSessions();
        ICollection<MovieSession> GetSessions(int movieId);

    }
}
