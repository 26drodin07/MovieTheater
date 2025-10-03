using Application.Interfaces;
using Application.Repos;
using Core.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
//db
builder.Services.AddDbContext<IAppDbContext,AppDbContext>(o => o.UseNpgsql(connection));

//UoW
builder.Services.AddScoped<IMovieRepo,MovieRepo>();
builder.Services.AddScoped<IMovieSessionRepo,MovieSessionRepo>();
builder.Services.AddScoped<ICinemaHallRepo,CinemaHallRepo>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

app.MapControllers();

app.Run();
