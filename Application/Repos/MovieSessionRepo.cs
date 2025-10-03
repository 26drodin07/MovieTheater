using Application.Interfaces;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repos
{
    public partial class MovieSessionRepo : IMovieSessionRepo
    {
        private readonly IAppDbContext _db;
        private readonly TimeInterval _outOfServiceInterval;
        public MovieSessionRepo(IAppDbContext db)
        {
            _db = db;
            _outOfServiceInterval = new("20:00", "8:00");
        }

        public async Task AddSession(MovieSession session)
        {
            var intervalsToCheck = GetRealTimeIntervals(session, _outOfServiceInterval);//Необходимо для сеансов которые идут несколько дней

            var sessionTimeInterval = new TimeInterval(session);
           
            var isDateValid = !_db.Sessions.Include(x => x.Movie).Include(x=>x.CinemaHall)
                .Any(x => !x.IsCanceled && intervalsToCheck.Any(i=> TimeInterval.DoOverlap(i,new(x))));
        }
        /// <summary>
        /// Получение интервалов сеанса из разных дней, с учетом часов после закрытия кинотеатра. 
        /// </summary>
        /// <param name="session">Сеанс, нужно сделать include Movie и CinemaHall</param>
        /// <param name="outOfServiceHours">часы закрытия</param>
        /// <returns></returns>
        public static List<TimeInterval> GetRealTimeIntervals(MovieSession session, TimeInterval outOfServiceHours)
        {
            if (!TimeInterval.DoOverlap(new(session), outOfServiceHours)) return [new(session)];

            var IntervalList = new List<TimeInterval>();
            var duration = session.Movie.Duration;
            var result = new TimeInterval(session);

            while (TimeInterval.DoOverlap(outOfServiceHours, result))
            {
                IntervalList.Add(new() { Start = result.Start, End = outOfServiceHours.Start });
                result.Start = outOfServiceHours.Start;
                result.End -= outOfServiceHours.Duration;
            }
            return IntervalList;
        }
       
        public Task<ICollection<MovieSession>> GetSessions()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<MovieSession>> GetSessionsByMovie(int movieId)
        {
            throw new NotImplementedException();
        }
    }
}
