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
