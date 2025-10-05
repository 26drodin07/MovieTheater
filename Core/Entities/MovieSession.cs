using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Сеанс
    /// </summary>
    public class MovieSession
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int CinemaHallId { get; set; }
        public CinemaHall CinemaHall { get; set; }
        public bool IsCanceled { get; set; }
        /// <summary>
        /// Дата с которой будет активен сеанс
        /// </summary>
        public DateTime ActivationDate { get; set; }
        /// <summary>
        /// Время начала (необходимо учитывать время работы кино, тех перерыв, длительность фильма)
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Сколько дней сеанс будет активен
        /// </summary>
        public int DurationInDays { get; set; }
        /// <summary>
        /// Цена по умолчанию, которую выставляем при созддании сеанса
        /// </summary>
        public decimal DefaultPrice { get; set; }
        /// <summary>
        /// Уникальные цены (на день и на промежуток x:до конца )
        /// </summary>
        public ICollection<PricePolicy> PricePolices { get; set; } = new List<PricePolicy>();
       /* public MovieSession(Movie movie, CinemaHall cinemaHall)
        { 
            Movie = movie;
            CinemaHall = cinemaHall;
        }*/
    }
}
