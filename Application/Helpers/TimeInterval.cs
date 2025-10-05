using Core.Models;

namespace Application.Repos
{
    public class TimeInterval
    {

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration { get; set; }


        public TimeInterval(DateTime start, TimeSpan duration)
        {
            Start = start;
            End = start+duration;
            Duration = duration;
        }
        public TimeInterval(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
            Duration = End - Start;
        }
        public TimeInterval(string start, string end)
        {
            Start = DateTime.Parse($"2000-01-01 {start}");
            End = DateTime.Parse($"2000-01-01 {end}");
            Duration = End - Start;
        }
        //public TimeInterval(MovieSession session)
        //{
        //    Start = session.StartTime;
        //    End = session.StartTime + session.Movie.Duration + session.CinemaHall.TechnicalBreakDuration;
        //    Duration = End - Start;
        //}
        /// <summary>
        /// Пересекаются ли промежутки
        /// </summary>
        /// <param name="interval1"></param>
        /// <param name="interval2"></param>
        /// <returns></returns>
        public static bool DoOverlap(TimeInterval interval1, TimeInterval interval2)
        {
            return interval1.Start < interval2.End && interval2.Start < interval1.End;
        }
        /// <summary>
        /// Пересекаются ли промежутки без учета даты (но с учетом что может быть переход через 00:00)
        /// </summary>
        /// <param name="interval1"></param>
        /// <param name="interval2"></param>
        /// <returns></returns>
        public static bool DoOverlapDaily(TimeInterval interval1, TimeInterval interval2)
        {
            var dailyTimeInterval1 = new TimeInterval(interval1.Start,interval1.End);
            var dailyTimeInterval2 = new TimeInterval(interval2.Start,interval2.End);
            dailyTimeInterval1.ClearDate();
            dailyTimeInterval2.ClearDate();

            return dailyTimeInterval1.Start < dailyTimeInterval2.End && dailyTimeInterval2.Start < dailyTimeInterval1.End;
        }
        public void ClearDate() 
        {
            Start = new(1, 1, 1, Start.Hour, Start.Minute, Start.Second);
            End = new(1, 1, 1, End.Hour, End.Minute, End.Second);
          
            if (Start < End)
                End.AddDays(1);
        }
    }
}
