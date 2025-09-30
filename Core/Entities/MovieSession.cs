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
        public DateTime StartDateTime { get; set; }
        public MovieSession(Movie movie, CinemaHall cinemaHall)
        { 
            Movie = movie;
            CinemaHall = cinemaHall;
        }
    }
}
