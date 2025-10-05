using Core.DTOs.Get;
using Core.DTOs.Patch;
using Core.DTOs.Post;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieTheater.Controllers
{
    /// <summary>
    /// Контроллер залов
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class MovieHallsController : ControllerBase
    {
        private readonly ICinemaHallRepo _cinemaHallRepo;
        public MovieHallsController(ICinemaHallRepo cinemaHallRepo)
        {
            _cinemaHallRepo = cinemaHallRepo;
        }
        [HttpGet]
        public async Task<ICollection<CinemaHallGetDTO>> GetHalls()
        {
            return await _cinemaHallRepo.GetHalls();
        }
        [HttpGet("{id}")]
        public async Task<CinemaHallGetDTO> GetHallById(int id)
        {
            return await _cinemaHallRepo.GetHallById(id);
        }
        [HttpPost]
        public async Task<CinemaHallGetDTO> PostHall(CinemaHallPostDTO dto)
        {
            return await _cinemaHallRepo.AddHall(dto);
        }
        [HttpPatch]
        public async Task<CinemaHallGetDTO> PatchHall(int hallId, CinemaHallPatchDTO dto)
        {
            return await _cinemaHallRepo.UpdateHall(dto, hallId);
        }
        [HttpDelete("{id}")]
        public async Task<int> DeleteHall(int id)
        {
            return await _cinemaHallRepo.DeleteHall(id);
        }
    }
}
