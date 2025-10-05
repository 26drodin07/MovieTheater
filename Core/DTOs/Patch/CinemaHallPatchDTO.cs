using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Patch
{
    public record CinemaHallPatchDTO(string? Name, int? PlaceCount, TimeSpan? TechnicalBreakDuration);
}
