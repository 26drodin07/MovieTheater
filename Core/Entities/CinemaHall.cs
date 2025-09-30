using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Кинозал
    /// </summary>
    public class CinemaHall
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PlaceCount { get; set; }
        public TimeSpan TechnicalBreakDuration { get; set; } = new();
        public ICollection<MovieSession> MovieSessions { get; set; } = new List<MovieSession>();

    }
}
