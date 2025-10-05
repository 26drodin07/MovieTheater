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
    public interface ICinemaHallRepo
    {
        Task<ICollection<CinemaHallGetDTO>> GetHalls();
        Task<CinemaHallGetDTO> GetHallById(int id);
        Task<CinemaHallGetDTO> AddHall(CinemaHallPostDTO dto);
        Task<CinemaHallGetDTO> UpdateHall(CinemaHallPatchDTO dto, int id);
        Task<int> DeleteHall(int id);
    }
}
