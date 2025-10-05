using Core.DTOs.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Post
{
    public record MoviePostDTO(
        string Name,
        int ReleaseDate,
        bool IsInTheaters,
        int PGRating,
        TimeSpan Duration,
        ICollection<int> MovieGenresIds);
}
