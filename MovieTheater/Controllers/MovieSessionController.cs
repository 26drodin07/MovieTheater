using Application.Repos;
using Core.DTOs;
using Core.DTOs.Get;
using Core.DTOs.Patch;
using Core.DTOs.Post;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MovieTheater.Controllers
{
    /// <summary>
    /// Контроллер сеансов
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieSessionController : ControllerBase
    {
        private readonly IMovieSessionRepo _sessionRepo;

        public MovieSessionController(IMovieSessionRepo movieSessionRepo)
        {
            _sessionRepo = movieSessionRepo;
        }

        /// <summary>
        /// Просто весь список без каких либо фильтров
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<MovieSessionGetDTO>> GetAll()
        {
            return await _sessionRepo.GetSessions();
        }
        /// <summary>
        /// Список активных сеансов с группировкой по фильмам и фильтрацией
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="searchPrompt"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<SessionGroupedByFilmsDTO>> GetFilteredGrouped(SessionFiltersDTO filters,string? searchPrompt)
        {
            return await _sessionRepo.GetFilteredSessions(true, filters);
        }

        [HttpGet("{id}")]
        public async Task<MovieSessionGetDTO> Get(int id)
        {
            return await _sessionRepo.GetSessionById(id);
        }
        [HttpGet("{id}")]
        public async Task<IEnumerable<MovieSessionGetDTO>> GetInMovie(int id)
        {
            return await _sessionRepo.GetSessionsByMovie(id);
        }

        [HttpPost]
        public async Task<MovieSessionGetDTO> Post(SessionPostDTO dto)
        {
            return await _sessionRepo.AddSession(dto);
        }

        [HttpPatch("{id}")]
        public async Task<MovieSessionGetDTO> Patch(int id, SessionPatchDTO dto)
        {
            return await _sessionRepo.UpdateSession(dto, id);

        }
        [HttpDelete("{id}")]
        public async Task<int> DeleteHall(int id)
        {
            return await _sessionRepo.DeleteSession(id);
        }
    }
}
