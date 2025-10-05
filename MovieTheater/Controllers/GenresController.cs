using Core.DTOs.Get;
using Core.DTOs.Post;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieTheater.Controllers
{
    /// <summary>
    /// Жанры
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IMovieRepo _movieRepo;
        public GenresController(IMovieRepo movieRepo)
        {
            _movieRepo = movieRepo;
        }
        /// <summary>
        /// Получить все жанры
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<GenreGetDTO>> GetGenres()
        {
            return await _movieRepo.GetGenres();
        }
        /// <summary>
        /// Добавить новый жанр
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GenreGetDTO> PostGenre(GenrePostDTO value)
        {
            return await _movieRepo.AddGenre(value);
        }
        /// <summary>
        /// Изменить жанр
        /// </summary>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<GenreGetDTO> PatchGenre(GenrePostDTO value, int id)
        {
            return await _movieRepo.UpdateGenre(value, id);
        }
        /// <summary>
        /// Удаление жанра
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<int> DeleteGenre(int id)
        {
            return await _movieRepo.RemoveGenre(id);
        }
    }
}
