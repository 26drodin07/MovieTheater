using Application.Repos;
using Core.DTOs;
using Core.DTOs.Get;
using Core.DTOs.Post;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class MovieSessionExtensions
    {
        public static IQueryable<MovieSession> ApplyAllFilters(this IQueryable<MovieSession> source, SessionFiltersDTO filters, decimal saleCoof = 1m)
        {
            var result = source.ApplyReleaseDateFilter(filters.ReleaseYearStart, filters.ReleaseYearEnd)
                            .ApplyDurationFilter(filters.durationMin, filters.durationMax)
                            .ApplyPegiFilter(filters.pegiMin, filters.pegiMax)
                            .ApplyPriceFilter(filters.priceMin, filters.priceMax, saleCoof)
                            .ApplyGenreFilter(filters.genreIds)
                            .ApplyHallsFilter(filters.hallIds)
                            .ApplyDateFilter(filters.IntervalDateStart,filters.IntervalDateEnd);
            return result;
        }
        public static IQueryable<MovieSession> ApplyDateFilter(this IQueryable<MovieSession> source, DateTime? startInterval, DateTime? endInterval)
        {

            if (startInterval is DateTime dt1 && startInterval is DateTime dt2)
            {
                if (dt1.Date==dt2.Date)
                    return source.Where(x=>x.ActivationDate > dt1.Date && 
                                            x.ActivationDate.AddDays(x.DurationInDays).Date < dt1.Date);

                TimeInterval searchInterval = new(dt1.Date, dt2.Date);
                return source.Where(x => TimeInterval.DoOverlap(searchInterval, new(x.ActivationDate.Date, x.ActivationDate.AddDays(x.DurationInDays).Date)));
            }
            if (startInterval is DateTime dtStart)
                return source.Where(x => x.ActivationDate.AddDays(x.DurationInDays).Date >= dtStart.Date);
            if (startInterval is DateTime dtEnd)
                return source.Where(x => x.ActivationDate.Date <= dtEnd.Date);
            return source;
        }
        public static IQueryable<MovieSession> ApplyReleaseDateFilter(this IQueryable<MovieSession> source, int? startYear, int? endYear)
        {
            var result = source;
            if (startYear != null)
                result.Where(x => x.Movie.ReleaseDate > startYear);
            if (endYear != null)
                result.Where(x => x.Movie.ReleaseDate < endYear);
            return result;
        }
        public static IQueryable<MovieSession> ApplyPegiFilter(this IQueryable<MovieSession> source, int? pegiMin, int? pegiMax)
        {
            var result = source;
            if (pegiMin != null)
                result.Where(x => x.Movie.PGRating > pegiMin);
            if (pegiMax != null)
                result.Where(x => x.Movie.PGRating < pegiMax);
            return result;
        }
        public static IQueryable<MovieSession> ApplyDurationFilter(this IQueryable<MovieSession> source, TimeSpan? durationMin, TimeSpan? durationMax)
        {
            var result = source;
            if (durationMin != null)
                result.Where(x => x.Movie.Duration > durationMin);
            if (durationMax != null)
                result.Where(x => x.Movie.Duration < durationMax);
            return result;
        }
        public static IQueryable<MovieSession> ApplyPriceFilter(this IQueryable<MovieSession> source,
                                                            decimal? priceMin,
                                                            decimal? priceMax,
                                                            decimal saleCoof = 1m)
        {
            var result = source;
            if (priceMin != null)
                result.Where(x => x.GetCurrentPrice(saleCoof) >= priceMin);
            if (priceMax != null)
                result.Where(x => x.GetCurrentPrice(saleCoof) <= priceMax);
            return result;
        }
        public static IQueryable<MovieSession> ApplyGenreFilter(this IQueryable<MovieSession> source, ICollection<int>? genreIds)
        {
            if (genreIds == null) return source;
            return source.Where(session => session.Movie.MovieGenres.Select(g => g.Id).Any(id => genreIds.Contains(id)));
        }
        public static IQueryable<MovieSession> ApplyHallsFilter(this IQueryable<MovieSession> source, ICollection<int>? hallIds)
        {
            if (hallIds == null) return source;
            return source.Where(session => hallIds.Contains(session.CinemaHallId));
        }
        /// <summary>
        /// Получаем текущую цену сеанса
        /// </summary>
        /// <param name="movieSession"></param>
        /// <param name="sale">коофициент скидки по умолчанию = 1</param>
        /// <returns></returns>
        public static decimal GetCurrentPrice(this MovieSession movieSession, decimal sale)
        {
            var pricePolicy = movieSession.GetActivePricePolicy();
            /*
                        if (movieSession.IsCanceled ||
                            movieSession.ActivationDate > DateTime.UtcNow ||
                            movieSession.ActivationDate + TimeSpan.FromDays(movieSession.DurationInDays) < DateTime.UtcNow)
                            return 0m;*/
            var result = pricePolicy?.Price ?? movieSession.DefaultPrice;
            return result * sale;
        }

        /// <summary>
        /// Текущая выставленная цена
        /// </summary>
        /// <param name="movieSession"></param>
        /// <returns></returns>
        public static PricePolicy? GetActivePricePolicy(this MovieSession movieSession)
        {
            return movieSession.PricePolices.SingleOrDefault(x =>
                 x.PolicyStart.Date == DateTime.UtcNow.Date ||
                 (x.IsToEnd && DateTime.UtcNow.Date > x.PolicyStart.Date));
        }
        /// <summary>
        /// Необходимо включить PricePolicies
        /// </summary>
        /// <param name="movieSession"></param>
        /// <param name="sale"></param>
        /// <returns></returns>
        public static MovieSessionGetDTO ToGetDTO(this MovieSession movieSession, decimal sale = 1m)
        {
            return new MovieSessionGetDTO(
                movieSession.Id,
                movieSession.MovieId,
                movieSession.CinemaHallId,
                movieSession.IsCanceled,
                movieSession.ActivationDate,
                movieSession.StartTime,
                movieSession.DurationInDays,
                movieSession.GetCurrentPrice(sale),
                movieSession.DefaultPrice);
        }
        public static IEnumerable<MovieSession> GetActive(this IEnumerable<MovieSession> source)
        {
            return source.Where(x => !x.IsCanceled);
        }
        public static MovieSession ToModel(this SessionPostDTO dto)
        {
            return new MovieSession()
            {
                ActivationDate = dto.ActivationDate,
                CinemaHallId = dto.CinemaHallId,
                IsCanceled = dto.IsCanceled,
                DefaultPrice = dto.DefaultPrice,
                DurationInDays = dto.DurationInDays,
                MovieId = dto.MovieId,
                StartTime = dto.StartTime,
            };
        }
    }
}
