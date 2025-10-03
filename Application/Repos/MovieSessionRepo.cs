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
    public class MovieSessionRepo : IMovieSessionRepo
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
           
            var isDateValid = !_db.Sessions.Include(x => x.Movie)
                .Any(x => !x.IsCanceled && intervalsToCheck.Any(i=>DoOverlap(i,new(x))));
        }
        private class TimeInterval
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public TimeSpan Duration { get; set; }

            public TimeInterval()
            {

            }
            public TimeInterval(string start, string end)
            {
                Start = DateTime.Parse($"2000-01-01 {start}");
                End = DateTime.Parse($"2000-01-01 {end}");
                Start.AddDays(-Start.Day);
                Start.AddYears(-Start.Year);
                Start.AddMonths(-Start.Month);
                End.AddDays(-End.Day);
                End.AddYears(-End.Year);
                End.AddMonths(-End.Month);

                if (Start < End)
                    End.AddDays(1);
                Duration = End - Start;
            }
            public TimeInterval(MovieSession session)
            {
                Start = session.StartDateTime;
                End = session.StartDateTime + session.Movie.Duration + session.CinemaHall.TechnicalBreakDuration;
                Duration = End - Start;
            }
        }
        private static List<TimeInterval> GetRealTimeIntervals(MovieSession session, TimeInterval outOfServiceHours)
        {
            if (!DoOverlap(new(session), outOfServiceHours)) return [new(session)];

            var IntervalList = new List<TimeInterval>();
            var duration = session.Movie.Duration;
            var result = new TimeInterval(session);

            while (DoOverlap(outOfServiceHours, result))
            {
                IntervalList.Add(new() { Start = result.Start, End = outOfServiceHours.Start });
                result.Start = outOfServiceHours.Start;
                result.End -= outOfServiceHours.Duration;
            }
            return IntervalList;
        }
        private static bool DoOverlap(TimeInterval interval1, TimeInterval interval2)
        {
            return interval1.Start < interval2.End && interval2.Start < interval1.End;
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
