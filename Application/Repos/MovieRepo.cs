using Application.Extensions;
using Application.Interfaces;
using Core.DTOs;
using Core.DTOs.Get;
using Core.DTOs.Patch;
using Core.DTOs.Post;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Repos
{
    public class MovieRepo : IMovieRepo
    {
        private readonly IAppDbContext _db;
        private readonly TimeInterval _outOfServiceInterval;

        public MovieRepo(IAppDbContext db, IConfiguration config)
        {
            _db = db;
            _outOfServiceInterval = new(config["WorkingHours:End"] ?? "8:00", config["WorkingHours:Start"] ?? "20:00");
        }



        public async Task<ICollection<MovieGetDTO>> GetMovies()
        {
            var sale = (await _db.SalePolicies.GetCurrentSaleAsync())?.Value ?? 1m;
            return await _db.Movies.Include(x => x.MovieGenres).Include(x => x.Sessions).OrderByFirstSession()
                .Select(x => x.ToGetDTO(sale)).ToListAsync();
        }

        public async Task<ICollection<MovieGetDTO>> GetFiltered(string? name, MovieFiltersDTO filters)
        {
            var sale = (await _db.SalePolicies.GetCurrentSaleAsync())?.Value ?? 1m;
            return await _db.Movies.Include(x => x.MovieGenres).Include(x => x.Sessions)
                .Search(name)
                .ApplyAllFilters(filters, sale).OrderByFirstSession()
                .Select(x => x.ToGetDTO(sale)).ToListAsync();
        }

        public async Task<MovieGetDTO> WithdrawFromDistibution(int movieId)
        {
            var sale = (await _db.SalePolicies.GetCurrentSaleAsync())?.Value ?? 1m;
            var movie = await _db.Movies.Include(x => x.Sessions).AsTracking().FirstOrDefaultAsync(x => x.Id == movieId);
            if (movie == null) throw new NotFoundException($"Фильма с id {movieId} не найдено");

            movie.IsInTheaters = false;
            foreach (var session in movie.Sessions)
            {
                session.IsCanceled = true;
            }
            await _db.SaveChangesAsync();
            return movie.ToGetDTO(sale);
        }
        public async Task<int> RemoveMovie(int movieId)
        {
            var movie = await _db.Movies.FirstOrDefaultAsync(x => x.Id == movieId);
            if (movie == null) throw new NotFoundException($"Фильма с id {movieId} не найдено");

            _db.Movies.Remove(movie);
            return await _db.SaveChangesAsync();
        }
        public async Task<MovieGetDTO> AddMovie(MoviePostDTO movieDto, byte[]? image)
        {
            var movieToAdd = movieDto.ToModel(image);
            //Добавим жанры которые есть в БД, возможно нужно кидать исключение если некорректный ID, надо уточнять
            var genresToAdd = await _db.Genres.AsTracking().Where(x => movieDto.MovieGenresIds.Contains(x.Id)).ToListAsync();
            movieToAdd.MovieGenres = genresToAdd;
            _db.Movies.Add(movieToAdd);
            await _db.SaveChangesAsync();
            return movieToAdd.ToGetDTO();//Скидка не нужна, тк сеансов у нового фильма не будет
        }
        public async Task<MovieGetDTO> UpdateMovie(MoviePatchDTO movieDto, byte[]? image, int id)
        {
            var sale = (await _db.SalePolicies.GetCurrentSaleAsync())?.Value ?? 1m;
            var movieToUpdate = _db.Movies.Include(x=>x.MovieGenres).Include(x=>x.Sessions)
                                                .AsTracking().SingleOrDefault(x=>x.Id == id);
            if(movieToUpdate == null) throw new NotFoundException($"Фильма с id {id} не найдено");

            movieToUpdate.Name = movieDto.Name ?? movieToUpdate.Name;
            movieToUpdate.PGRating = movieDto.PGRating ?? movieToUpdate.PGRating;
            movieToUpdate.ReleaseDate = movieDto.ReleaseDate ?? movieToUpdate.ReleaseDate;
            movieToUpdate.Image = image ?? movieToUpdate.Image;
            movieToUpdate.IsInTheaters = movieDto.IsInTheaters ?? movieToUpdate.IsInTheaters;

            if (movieDto.IsInTheaters is false) 
            {
                foreach (var session in movieToUpdate.Sessions) 
                    session.IsCanceled = true;
            }

            if (movieDto.Duration != null && 
                movieDto.Duration != movieToUpdate.Duration && 
                movieToUpdate.IsInTheaters == true) // При изменении длительности фильма необходима повторная валидация сеансов
            {
                foreach (var session in movieToUpdate.Sessions)
                    SessionScheduleValidation(session, movieToUpdate);
                movieToUpdate.Duration = movieDto.Duration ?? movieToUpdate.Duration;
            }

            await _db.SaveChangesAsync();
            return movieToUpdate.ToGetDTO(sale);
        }

        public async Task<ICollection<GenreGetDTO>> GetGenres()
        {
            return await _db.Genres.Select(x => x.ToGetDTO()).ToListAsync();
        }

        public async Task<GenreGetDTO> AddGenre(GenrePostDTO dto)
        {
            var GenreToAdd = new MovieGenre() { Name = dto.Name };
            _db.Genres.Add(GenreToAdd);
            await _db.SaveChangesAsync();
            return GenreToAdd.ToGetDTO();
        }

        public async Task<GenreGetDTO> UpdateGenre(GenrePostDTO genre, int id)
        {
            var GenreToUpdate = await _db.Genres.AsTracking().SingleOrDefaultAsync(x => x.Id == id);
            if (GenreToUpdate == null)
                throw new NotFoundException($"Жанра с id {id} не найдено");

            GenreToUpdate.Name = genre.Name;
            await _db.SaveChangesAsync();
            return GenreToUpdate.ToGetDTO();
        }


        private void SessionScheduleValidation(MovieSession session, Movie movie)
        {
            var movieHall = _db.CinemaHalls
                .Include(ch => ch.MovieSessions).ThenInclude(x => x.Movie)
                .SingleOrDefault(x => x.Id == session.CinemaHallId);
            if (movieHall == null)
                throw new NotFoundException($"No movieHall with id {session.CinemaHallId}");
            TimeInterval movieInterval = new(session.StartTime, movie.Duration + movieHall.TechnicalBreakDuration);
            TimeInterval sessionInterval = new(session.ActivationDate, session.ActivationDate.AddDays(session.DurationInDays));

            var overlapActiveDaysSessions = movieHall.MovieSessions.Where(x =>
                    !x.IsCanceled && TimeInterval.DoOverlap(sessionInterval, new(x.ActivationDate, x.ActivationDate.AddDays(x.DurationInDays)))); //только сеансы у которых пересечение по дням

            var overlapDailySessions = overlapActiveDaysSessions.Where(x =>
                    TimeInterval.DoOverlapDaily(movieInterval, new(x.StartTime, x.Movie.Duration + movieHall.TechnicalBreakDuration))); //ищем пересечение интервалов текущего сеанса
            if (overlapDailySessions.Any())
                throw new BadRequestException($"При изменении длительности фильма возникли пересечения сеансов");
        }

    }
}
