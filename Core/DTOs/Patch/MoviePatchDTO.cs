using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Patch
{
    public record MoviePatchDTO(
    string? Name,
    int? ReleaseDate,
    bool? IsInTheaters,
    int? PGRating,
    TimeSpan? Duration,
    ICollection<int>? MovieGenresIds);
}
