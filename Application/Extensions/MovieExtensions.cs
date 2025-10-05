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
using static System.Net.Mime.MediaTypeNames;

namespace Application.Extensions
{
    public static class MovieExtensions
    {
        public static IQueryable<Movie> ApplyAllFilters(this IQueryable<Movie> source, MovieFiltersDTO filters, decimal saleCoof = 1m)
        {
            var result = source.ApplyDateFilters(filters.ReleaseYearStart, filters.ReleaseYearEnd)
                            .ApplyDurationFilters(filters.durationMin, filters.durationMax)
                            .ApplyPegiFilters(filters.pegiMin, filters.pegiMax)
                            .ApplyPriceFilters(filters.priceMin, filters.priceMax, saleCoof)
                            .ApplyGenreFilters(filters.genreIds);
            return result;
        }
        public static IQueryable<Movie> ApplyDateFilters(this IQueryable<Movie> source, int? startYear, int? endYear)
        {
            var result = source;
            if (startYear != null)
                result.Where(x => x.ReleaseDate > startYear);
            if (endYear != null)
                result.Where(x => x.ReleaseDate < endYear);
            return result;
        }
        public static IQueryable<Movie> ApplyPegiFilters(this IQueryable<Movie> source, int? pegiMin, int? pegiMax)
        {
            var result = source;
            if (pegiMin != null)
                result.Where(x => x.PGRating > pegiMin);
            if (pegiMax != null)
                result.Where(x => x.PGRating < pegiMax);
            return result;
        }
        public static IQueryable<Movie> ApplyDurationFilters(this IQueryable<Movie> source, TimeSpan? durationMin, TimeSpan? durationMax)
        {
            var result = source;
            if (durationMin != null)
                result.Where(x => x.Duration > durationMin);
            if (durationMax != null)
                result.Where(x => x.Duration < durationMax);
            return result;
        }
        public static IQueryable<Movie> ApplyPriceFilters(this IQueryable<Movie> source,
                                                            decimal? priceMin,
                                                            decimal? priceMax,
                                                            decimal saleCoof = 1m)
        {
            var result = source;
            if (priceMin != null)
                result.Where(m => m.Sessions.Min(ms => ms.GetCurrentPrice(saleCoof))  > priceMin);
            if (priceMax != null)
                result.Where(m => m.Sessions.Max(ms => ms.GetCurrentPrice(saleCoof))  < priceMax);
            return result;
        }
        public static IQueryable<Movie> ApplyGenreFilters(this IQueryable<Movie> source, ICollection<int>? genreIds)
        {
            if (genreIds == null) return source;
            return source.Where(movie => movie.MovieGenres.Select(g => g.Id).Any(id => genreIds.Contains(id)));
        }

        public static IQueryable<Movie> Search(this IQueryable<Movie> source, string prompt)
        {
            return source.Search(prompt);
        }

        /// <summary>
        /// Преобразование к DTO (В query нужны Genres и Sessions)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static MovieGetDTO ToGetDTO(this Movie source, decimal sale = 1m)
        {
            var closestSession = source.Sessions.getNearestSession();


            return new MovieGetDTO(
                source.Id,
                source.Name,
                source.ReleaseDate,
                source.IsInTheaters,
                source.PGRating,
                source.Duration,
                source.MovieGenres.Select(x => x.ToGetDTO()).ToList(),
                source.Image,
                closestSession?.ToGetDTO(sale));
        }

        public static GenreGetDTO ToGetDTO(this MovieGenre source)
        {
            return new GenreGetDTO(source.Id, source.Name);
        }

        public static Movie ToModel(this MoviePostDTO dto, byte[]? bytes) 
        {
            Movie model = new() 
                {
                    Duration = dto.Duration,
                    IsInTheaters = dto.IsInTheaters,
                    Name = dto.Name,
                    PGRating = dto.PGRating,
                    ReleaseDate = dto.ReleaseDate,
                    Image = bytes
                };
            return model;
        }
        public static MovieSession? getNearestSession(this IEnumerable<MovieSession> source)
        {
            var currentDT = DateTime.UtcNow;
            var nonCanceledSessions = source.Where(x => !x.IsCanceled);
            var activeSessions = nonCanceledSessions.Where(x => x.ActivationDate < currentDT &&
                                                    x.ActivationDate.AddDays(x.DurationInDays).AddTime(x.StartTime) > currentDT); //Если сеанс уже был активирован И последний показ еще не начался
            var futureSessons = nonCanceledSessions.Where(x => x.ActivationDate > currentDT);

            var nearestActiveSession = activeSessions.OrderBy(x => x.StartTime).FirstOrDefault();
            if (nearestActiveSession != null) return nearestActiveSession;

            return futureSessons.OrderBy(x => x.ActivationDate).ThenBy(x => x.StartTime).FirstOrDefault();
        }

    }
}
