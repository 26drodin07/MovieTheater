using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    /// <summary>
    /// Для фильтрации при поиске фильмов
    /// </summary>
    public record MovieFiltersDTO(int? genreId, int ReleaseYear);
}
