using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Get
{
    public record CinemaHallGetDTO(int id,string Name, int PlaceCount, TimeSpan TechnicalBreakDuration);
}
