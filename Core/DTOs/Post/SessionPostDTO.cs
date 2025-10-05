using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Post
{
    public record SessionPostDTO(
    int MovieId,
    int CinemaHallId,
    bool IsCanceled,
    DateTime ActivationDate,
    /// <summary>
    /// Время начала (необходимо учитывать время работы кино, тех перерыв, длительность фильма)
    /// </summary>
    DateTime StartTime,
    /// <summary>
    /// Сколько дней сеанс будет активен
    /// </summary>
    int DurationInDays,
    /// <summary>
    /// Цена по умолчанию, которую выставляем при созддании сеанса
    /// </summary>
    decimal DefaultPrice);
}
