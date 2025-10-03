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
    public record MovieFiltersDTO(
        List<int>? genreIds, 
        int? ReleaseYearStart,
        int? ReleaseYearEnd,
        byte? pegiMin,
        byte? pegiMax,
        TimeSpan? durationMin,
        TimeSpan? durationMax,
        decimal? priceMin,
        decimal? priceMax
    );
}
