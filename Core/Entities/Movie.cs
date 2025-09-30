using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Фильм
    /// </summary>
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly ReleaseDate { get; set; } = new();
        /// <summary>
        /// В прокате ли фильм
        /// </summary>
        public bool IsInTheaters { get; set; } = false;

        public int PGRating { get; set; } = 0;
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(0);
        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
        public byte[]? Image { get; set; }

        public ICollection<MovieSession> Sessions { get; set; } = new List<MovieSession>();
    }
}
