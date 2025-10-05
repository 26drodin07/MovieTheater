using Core.DTOs.Get;
using Core.DTOs.Patch;
using Core.DTOs.Post;
using Core.DTOs.Put;
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
        Task<ICollection<MovieSessionGetDTO>> GetSessions();
        Task<MovieSessionGetDTO> GetSessionById(int id);
        Task<ICollection<MovieSessionGetDTO>> GetSessionsByMovie(int movieId);
        Task<ICollection<SessionGroupedByFilmsDTO>> GetGroupedByFilm();
        Task<MovieSessionGetDTO> AddSession(SessionPostDTO movieSession);
        Task<MovieSessionGetDTO> UpdateSession(SessionPatchDTO movieSession);
        Task<int> DeleteSession(int id);
        //Кастомные цены
        Task<MovieSessionGetDTO> AddCustomPrice(int sessionId, PricePolicyPostDTO priceDto);
        Task<ICollection<PricePolicyGetDTO>> GetCustomPrices(int sessionId);
        Task<PricePolicyGetDTO> UpdateCustomPrice(int id, int sessionId, PricePolicyPatchDTO priceDto);
        Task<int> DeleteCustomPrice(int id);

        Task<SalePolicyGetDTO> PutSale(SalePolicyPutDTO dto);
        Task<ICollection<SalePolicyGetDTO>> GetSales(); 
    }
}
