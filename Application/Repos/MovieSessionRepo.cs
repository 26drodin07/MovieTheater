using Application.Extensions;
using Application.Interfaces;
using Core.DTOs.Get;
using Core.DTOs.Patch;
using Core.DTOs.Post;
using Core.DTOs.Put;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Application.Repos
{
    public partial class MovieSessionRepo : IMovieSessionRepo
    {
        private readonly IAppDbContext _db;
        private readonly TimeInterval _outOfServiceInterval;
        public MovieSessionRepo(IAppDbContext db, IConfiguration config)
        {
            _db = db;
            _outOfServiceInterval = new(config["WorkingHours:End"] ?? "8:00", config["WorkingHours:Start"] ?? "20:00");
        }

        public async Task<MovieSessionGetDTO> AddSession(SessionPostDTO dto)
        {
            var sale = (await _db.SalePolicies.GetCurrentSaleAsync())?.Value ?? 1m;
            var Movie = await _db.Movies.SingleOrDefaultAsync(x => x.Id == dto.MovieId);
            if (Movie == null) throw new NotFoundException($"No movie with id {dto.MovieId}");
            var CinemaHall = await _db.CinemaHalls
                .Include(ch => ch.MovieSessions).ThenInclude(x => x.Movie)
                .SingleOrDefaultAsync(x => x.Id == dto.CinemaHallId);
            if (CinemaHall == null)
                throw new NotFoundException($"No movieHall with id {dto.CinemaHallId}");
            var modelToAdd = dto.ToModel();
            modelToAdd.CinemaHall = CinemaHall;
            modelToAdd.Movie = Movie;
            SessionScheduleValidation(modelToAdd);
            _db.Sessions.Add(modelToAdd);
            await _db.SaveChangesAsync();
            return modelToAdd.ToGetDTO(sale);
        }
        private void SessionScheduleValidation(MovieSession session)
        {
            var movieHall = session.CinemaHall;
            TimeInterval movieInterval = new(session.StartTime, session.Movie.Duration + movieHall.TechnicalBreakDuration);
            TimeInterval sessionInterval = new(session.ActivationDate, session.ActivationDate.AddDays(session.DurationInDays));
            if (TimeInterval.DoOverlapDaily(_outOfServiceInterval, movieInterval))
                throw new BadRequestException("Некорректное время начала");

            var overlapActiveDaysSessions = movieHall.MovieSessions.Where(x =>
                    !x.IsCanceled && TimeInterval.DoOverlap(sessionInterval, new(x.ActivationDate, x.ActivationDate.AddDays(x.DurationInDays)))); //только сеансы у которых пересечение по дням


            var overlapDailySessions = overlapActiveDaysSessions.Where(x =>
                    TimeInterval.DoOverlapDaily(movieInterval, new(x.StartTime, x.Movie.Duration + movieHall.TechnicalBreakDuration))); //ищем пересечение интервалов текущего сеанса
            if (overlapDailySessions.Any())
                throw new BadRequestException($"На указаный промежуток дат и времени начала уже назначены сеансы");
        }

        public async Task<ICollection<MovieSessionGetDTO>> GetSessions()
        {
            var sale = (await _db.SalePolicies.GetCurrentSaleAsync())?.Value ?? 1m;
            return await _db.Sessions.Include(x => x.PricePolices)
                .Select(x => x.ToGetDTO(sale)).ToListAsync();
        }

        public async Task<ICollection<MovieSessionGetDTO>> GetSessionsByMovie(int movieId)
        {
            var sale = (await _db.SalePolicies.GetCurrentSaleAsync())?.Value ?? 1m;
            var movie = await _db.Movies.FirstOrDefaultAsync(x => x.Id == movieId);
            if (movie == null) throw new NotFoundException($"Фильма с id {movieId} не найдено");
            return await _db.Sessions.Include(x => x.PricePolices)
                .Where(x => x.MovieId == movieId)
                .Select(x => x.ToGetDTO(sale)).ToListAsync();
        }

        public async Task<MovieSessionGetDTO> GetSessionById(int id)
        {
            var sale = (await _db.SalePolicies.GetCurrentSaleAsync())?.Value ?? 1m;
            var result = await _db.Sessions.Include(x => x.PricePolices)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (result == null) throw new NotFoundException($"Сеанса с id {id} не найдено");

            return result.ToGetDTO(sale);
        }

        public async Task<MovieSessionGetDTO> UpdateSession(SessionPatchDTO dto, int id)
        {
            var sessionToUpdate = await _db.Sessions.AsTracking()
                .SingleOrDefaultAsync(x => x.Id == id); ;
            if (sessionToUpdate == null) throw new NotFoundException($"Сеанса с id {id} не найдено");


            var sale = (await _db.SalePolicies.GetCurrentSaleAsync())?.Value ?? 1m;

            if (dto.MovieId != null)
            {
                var Movie = await _db.Movies.SingleOrDefaultAsync(x => x.Id == dto.MovieId);
                if (Movie == null) throw new NotFoundException($"No movie with id {dto.MovieId}");
                sessionToUpdate.Movie = Movie;
            }
            if (dto.CinemaHallId != null)
            {
                var CinemaHall = await _db.CinemaHalls
                    .Include(ch => ch.MovieSessions).ThenInclude(x => x.Movie)
                    .SingleOrDefaultAsync(x => x.Id == dto.CinemaHallId);
                if (CinemaHall == null)
                    throw new NotFoundException($"No movieHall with id {dto.CinemaHallId}");
                sessionToUpdate.CinemaHall = CinemaHall;
            }
            sessionToUpdate.IsCanceled = dto.IsCanceled ?? sessionToUpdate.IsCanceled;
            sessionToUpdate.StartTime = dto.StartTime ?? sessionToUpdate.StartTime;
            sessionToUpdate.DurationInDays = dto.DurationInDays ?? sessionToUpdate.DurationInDays;
            sessionToUpdate.ActivationDate = dto.ActivationDate ?? sessionToUpdate.ActivationDate;
            sessionToUpdate.DefaultPrice = dto.DefaultPrice ?? sessionToUpdate.DefaultPrice;

            SessionScheduleValidation(sessionToUpdate);
            await _db.SaveChangesAsync();
            return sessionToUpdate.ToGetDTO(sale);
        }

        public async Task<ICollection<SessionGroupedByFilmsDTO>> GetGroupedByFilm()
        {
            var sale = (await _db.SalePolicies.GetCurrentSaleAsync())?.Value ?? 1m;
            return await _db.Sessions.Include(x => x.PricePolices).Select(x => x.ToGetDTO(sale))
                .GroupBy(x => x.MovieId)
                .Select(x => new SessionGroupedByFilmsDTO(x.Key, x.ToList())).ToListAsync();
        }

        public async Task<int> DeleteSession(int id)
        {
            var sessionToDelete = await _db.Sessions.AsTracking()
                .SingleOrDefaultAsync(x => x.Id == id); ;
            if (sessionToDelete == null) throw new NotFoundException($"Сеанса с id {id} не найдено");
            _db.Sessions.Remove(sessionToDelete);
            await _db.SaveChangesAsync();
            return sessionToDelete.Id;
        }

        public async Task<MovieSessionGetDTO> AddCustomPrice(int sessionId, PricePolicyPostDTO priceDto)
        {
            var session = await _db.Sessions.Include(x => x.PricePolices).AsTracking()
                .SingleOrDefaultAsync(x => x.Id == sessionId); ;
            if (session == null) throw new NotFoundException($"Сеанса с id {sessionId} не найдено");

            PricePolicy policy = new()
            {
                IsToEnd = priceDto.IsToEnd,
                PolicyStart = priceDto.startDate,
                Price = priceDto.price
            };
            if (priceDto.IsToEnd == true &&
                session.PricePolices.Any(x => x.IsToEnd))
                throw new BadRequestException($"Цена до конца сеанса уже назначена");
            if (session.PricePolices.Any(x => x.PolicyStart.Date == priceDto.startDate.Date))
                throw new BadRequestException($"На данную дату уже назначена цена");

            session.PricePolices.Add(policy);
            await _db.SaveChangesAsync();
            var sale = (await _db.SalePolicies.GetCurrentSaleAsync())?.Value ?? 1m;
            return session.ToGetDTO(sale);

        }

        public async Task<ICollection<PricePolicyGetDTO>> GetCustomPrices(int sessionId)
        {
            var session = await _db.Sessions.Include(x => x.PricePolices).AsTracking()
               .SingleOrDefaultAsync(x => x.Id == sessionId); ;
            if (session == null) throw new NotFoundException($"Сеанса с id {sessionId} не найдено");

            return session.PricePolices
                .Select(x => new PricePolicyGetDTO(x.Id, x.PolicyStart, x.IsToEnd, x.Price)).ToList();
        }

        public async Task<PricePolicyGetDTO> UpdateCustomPrice(int id, PricePolicyPatchDTO priceDto)
        {
            var сustomPrice = await _db.PricePolicies.AsTracking().SingleOrDefaultAsync(x => x.Id == id);
            if (сustomPrice == null) throw new NotFoundException($"Цены с id {id} не найдено");

            var session = await _db.Sessions.Include(x => x.PricePolices)
               .SingleOrDefaultAsync(x => x.Id == сustomPrice.MovieSessionId);
            if (session == null) throw new NotFoundException($"Не найден сеанс у цены с id {id}");

            if (priceDto.IsToEnd is true &&
                session.PricePolices.Any(x => x.IsToEnd && x.Id != id))
                throw new BadRequestException($"Цена до конца сеанса уже назначена");
            if (priceDto.startDate is DateTime dtoDate &&
                session.PricePolices.Any(x => x.PolicyStart.Date == dtoDate && x.Id != id))
                throw new BadRequestException($"На данную дату уже назначена цена");
            сustomPrice.PolicyStart = priceDto.startDate ?? сustomPrice.PolicyStart;
            сustomPrice.Price = priceDto.Price ?? сustomPrice.Price;
            сustomPrice.IsToEnd = priceDto.IsToEnd ?? сustomPrice.IsToEnd;
            await _db.SaveChangesAsync();
            return new PricePolicyGetDTO(сustomPrice.Id,
                                            сustomPrice.PolicyStart,
                                            сustomPrice.IsToEnd,
                                            сustomPrice.Price);

        }

        public async Task<int> DeleteCustomPrice(int id)
        {
            var сustomPrice = await _db.PricePolicies.SingleOrDefaultAsync(x => x.Id == id);
            if (сustomPrice == null) throw new NotFoundException($"Цены с id {id} не найдено");
            _db.PricePolicies.Remove(сustomPrice);
            await _db.SaveChangesAsync();
            return id;
        }

        public async Task<SalePolicyGetDTO> PutSale(SalePolicyPutDTO dto)
        {
            var putSale = _db.SalePolicies.AsTracking().SingleOrDefault(x=>x.PolicyStart.Date == dto.startDate.Date);
            if (putSale == null)
            {
                putSale = new SalePolicy() { PolicyStart = dto.startDate.Date, Value = dto.saleCoof };
                _db.SalePolicies.Add(putSale);
            }
            else
            { 
                putSale.PolicyStart = dto.startDate.Date;
                putSale.Value = dto.saleCoof;
            }
            await _db.SaveChangesAsync();
            return new(putSale.Id, putSale.PolicyStart, putSale.Value);
        }

        public async Task<ICollection<SalePolicyGetDTO>> GetSales()
        {
            return await _db.SalePolicies
                .Select(x => new SalePolicyGetDTO(x.Id, x.PolicyStart, x.Value)).ToListAsync();
        }
    }
}
