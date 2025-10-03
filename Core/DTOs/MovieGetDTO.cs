using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public record MovieGetDTO (  
        int Id,
        string Name,
        int ReleaseDate,
        bool IsInTheaters,
        int PGRating,
        TimeSpan Duration,
        ICollection<GenreGetDTO> MovieGenres,
        byte[]? Image,
        MovieSessionGetDTO ClosestSession
    );
}
