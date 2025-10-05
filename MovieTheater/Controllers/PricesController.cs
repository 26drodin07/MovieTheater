using Application.Repos;
using Core.DTOs.Get;
using Core.DTOs.Patch;
using Core.DTOs.Post;
using Core.DTOs.Put;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieTheater.Controllers
{
    /// <summary>
    /// Контроллер кастомных цен/скидок
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly IMovieSessionRepo _sessionRepo;
        public PricesController(IMovieSessionRepo sessionRepo)
        {
            _sessionRepo = sessionRepo;
        }
        [HttpGet("{sessionId}")]
        public async Task<ICollection<PricePolicyGetDTO>> GetCustomPricesInSession(int sessionId)
        {
            return await _sessionRepo.GetCustomPrices(sessionId);
        }
        [HttpPost]
        public async Task<MovieSessionGetDTO> PostPrice(int sessionId, PricePolicyPostDTO priceDto)
        {
            return await _sessionRepo.AddCustomPrice(sessionId, priceDto);
        }
        [HttpPatch]
        public async Task<PricePolicyGetDTO> PatchPrice(int priceId, PricePolicyPatchDTO priceDto)
        {
            return await _sessionRepo.UpdateCustomPrice(priceId, priceDto);
        }
        [HttpDelete("{id}")]
        public async Task<int> DeleteCustomPrice(int id)
        {
            return await _sessionRepo.DeleteCustomPrice(id);
        }
        /// <summary>
        /// Назначение скидки - если сеидка на эту дату уже есть то будет изменена, если нет - создана
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<SalePolicyGetDTO> PutSale(SalePolicyPutDTO dto)
        {
            return await _sessionRepo.PutSale(dto);
        }
        [HttpGet]
        public async Task<ICollection<SalePolicyGetDTO>> GetSales()
        {
            return await _sessionRepo.GetSales();
        }        
    }
}
