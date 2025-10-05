using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public record SessionFiltersDTO(
        List<int>? genreIds,
        List<int>? hallIds,
        int? ReleaseYearStart,
        int? ReleaseYearEnd,
        byte? pegiMin,
        byte? pegiMax,
        TimeSpan? durationMin,
        TimeSpan? durationMax,
        decimal? priceMin,
        decimal? priceMax,
        DateTime? StartDate,
        DateTime? IntervalDateStart,
        DateTime? IntervalDateEnd
    );
}
