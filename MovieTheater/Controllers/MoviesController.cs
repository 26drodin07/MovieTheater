using Core.DTOs;
using Core.DTOs.Get;
using Core.DTOs.Patch;
using Core.DTOs.Post;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieTheater.Controllers
{
    /// <summary>
    /// Контролер фильмов
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepo _movieRepo;
        public MoviesController(IMovieRepo movieRepo)  
        {
            _movieRepo = movieRepo;
        }


        [HttpGet]
        public async Task<IEnumerable<MovieGetDTO>> GetMovies()
        {
            return await _movieRepo.GetMovies();
        }
        /// <summary>
        /// Поиск кино с фильтрацией
        /// </summary>
        /// <param name="dto">фильтры</param>
        /// <param name="searchNamePrompt">подстрока имени</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MovieGetDTO>> GetFilteredMovies(MovieFiltersDTO dto, string? searchNamePrompt)
        {
            return await _movieRepo.GetFiltered(searchNamePrompt,dto);
        }
        /// <summary>
        /// Снять с проката. Также можно это сделать в методе patch полем IsInTheaters, не стал убирать
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MovieGetDTO> WithdrawFromDistibution(int id)
        {
            return await _movieRepo.WithdrawFromDistibution(id);
        }
        /// <summary>
        /// Создание фильма
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MovieGetDTO> PostMovie([FromForm] MoviePostDTO dto, IFormFile? file)
        {
            byte[]? fileBytes = null;
            if (file != null && file.Length != 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }
            }
            return await _movieRepo.AddMovie(dto, fileBytes);
        }
        /// <summary>
        /// Обновление фильма
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<MovieGetDTO> PatchMovie([FromForm]MoviePatchDTO dto, [FromForm] int id, IFormFile? file)
        {
            byte[]? fileBytes = null;
            if (file != null && file.Length != 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }
            }
            return await _movieRepo.UpdateMovie(dto, fileBytes, id);
        }
        /// <summary>
        /// Удаление фильма
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await _movieRepo.RemoveMovie(id);
        }
        
    }
}
