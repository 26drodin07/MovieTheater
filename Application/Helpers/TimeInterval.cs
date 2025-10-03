using Core.Models;

namespace Application.Repos
{
    public class TimeInterval
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
        public static bool DoOverlap(TimeInterval interval1, TimeInterval interval2)
        {
            return interval1.Start < interval2.End && interval2.Start < interval1.End;
        }
    }
}
