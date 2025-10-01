using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IMovieSessionRepo
    {
        Task<ICollection<MovieSession>> GetSessions();
        Task<ICollection<MovieSession>> GetSessionsByMovie(int movieId);
    }
}
