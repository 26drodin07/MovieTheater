using Application.Interfaces;
using Core.DTOs.Get;
using Core.DTOs.Patch;
using Core.DTOs.Post;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repos
{
    public class CinemaHallRepo : ICinemaHallRepo
    {
        private readonly IAppDbContext _db;
        public CinemaHallRepo(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<CinemaHallGetDTO> AddHall(CinemaHallPostDTO dto)
        {
            var hallToAdd = new CinemaHall()
            {
                Name = dto.Name,
                PlaceCount = dto.PlaceCount,
                TechnicalBreakDuration = dto.TechnicalBreakDuration,
            };
            _db.CinemaHalls.Add(hallToAdd);
            await _db.SaveChangesAsync();
            return new(hallToAdd.Id, hallToAdd.Name, hallToAdd.PlaceCount, hallToAdd.TechnicalBreakDuration);
        }

        public async Task<int> DeleteHall(int id)
        {
            var hallToDelete = await _db.CinemaHalls.SingleOrDefaultAsync(x => x.Id == id);
            if (hallToDelete == null)
                throw new NotFoundException($"Зала с id {id} не найдено");
            _db.CinemaHalls.Remove(hallToDelete);
            await _db.SaveChangesAsync();
            return id;
        }

        public async Task<CinemaHallGetDTO> GetHallById(int id)
        {
            var hall = await _db.CinemaHalls.SingleOrDefaultAsync(x => x.Id == id);
            if (hall == null)
                throw new NotFoundException($"Зала с id {id} не найдено");
            return new(hall.Id, hall.Name, hall.PlaceCount, hall.TechnicalBreakDuration);
        }

        public async Task<ICollection<CinemaHallGetDTO>> GetHalls()
        {
            return await _db.CinemaHalls.Select(hall=> new CinemaHallGetDTO(hall.Id, hall.Name, hall.PlaceCount, hall.TechnicalBreakDuration)).ToListAsync();
        }

        public async Task<CinemaHallGetDTO> UpdateHall(CinemaHallPatchDTO dto, int id)
        {
            var hallToUpdate = await _db.CinemaHalls.AsTracking().SingleOrDefaultAsync(x => x.Id == id);
            if (hallToUpdate == null)
                throw new NotFoundException($"Зала с id {id} не найдено");
            hallToUpdate.TechnicalBreakDuration = dto.TechnicalBreakDuration ?? hallToUpdate.TechnicalBreakDuration;
            hallToUpdate.Name = dto.Name ?? hallToUpdate.Name;
            hallToUpdate.PlaceCount = dto.PlaceCount ?? hallToUpdate.PlaceCount;
            await _db.SaveChangesAsync();
            return new(hallToUpdate.Id, hallToUpdate.Name, hallToUpdate.PlaceCount, hallToUpdate.TechnicalBreakDuration);
        }
    }
}
