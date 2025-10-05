using Core.DTOs;
using Core.DTOs.Get;
using Core.DTOs.Patch;
using Core.DTOs.Post;
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
        /// <summary>
        /// Полный список фильмовя
        /// </summary>
        /// <returns></returns>
        Task<ICollection<MovieGetDTO>> GetMovies();
        /// <summary>
        /// Поиск с фильтрами
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        Task<ICollection<MovieGetDTO>> GetFiltered(string name, MovieFiltersDTO filters);
        /// <summary>
        /// Снять с проката
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        Task<MovieGetDTO> WithdrawFromDistibution(int movieId);

        Task<MovieGetDTO> AddMovie(MoviePostDTO movie, byte[]? file);
        Task<MovieGetDTO> UpdateMovie(MoviePatchDTO movie, byte[]? file, int id);
        Task<int> RemoveMovie(int id);

        
        Task<ICollection<GenreGetDTO>> GetGenres();
        Task<GenreGetDTO> AddGenre(GenrePostDTO genre);
        Task<GenreGetDTO> UpdateGenre(GenrePostDTO genre, int id);


    }
}
