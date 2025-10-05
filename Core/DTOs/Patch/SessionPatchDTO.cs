using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Patch
{
    public record SessionPatchDTO(
    int? MovieId,
    int? CinemaHallId,
    bool? IsCanceled,
    DateTime? ActivationDate,
    /// <summary>
    /// Время начала (необходимо учитывать время работы кино, тех перерыв, длительность фильма)
    /// </summary>
    DateTime? StartTime,
    /// <summary>
    /// Сколько дней сеанс будет активен
    /// </summary>
    int? DurationInDays,
    /// <summary>
    /// Цена по умолчанию
    /// </summary>
    decimal? DefaultPrice);
}
