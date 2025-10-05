using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Get
{
    public record MovieSessionGetDTO
    (
        int Id,
        int MovieId,
        int CinemaHallId,
        bool IsCanceled,
        DateTime ActivationDate,
        DateTime StartTime,
        int DurationInDays,
        decimal Price,
        decimal DefaultPrice
    );
}
