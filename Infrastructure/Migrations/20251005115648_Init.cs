using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CinemaHalls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PlaceCount = table.Column<int>(type: "integer", nullable: false),
                    TechnicalBreakDuration = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaHalls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<int>(type: "integer", nullable: false),
                    IsInTheaters = table.Column<bool>(type: "boolean", nullable: false),
                    PGRating = table.Column<int>(type: "integer", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalePolicies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    PolicyStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalePolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieMovieGenre",
                columns: table => new
                {
                    MovieGenresId = table.Column<int>(type: "integer", nullable: false),
                    MoviesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieMovieGenre", x => new { x.MovieGenresId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_MovieMovieGenre_Genres_MovieGenresId",
                        column: x => x.MovieGenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieMovieGenre_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MovieId = table.Column<int>(type: "integer", nullable: false),
                    CinemaHallId = table.Column<int>(type: "integer", nullable: false),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    ActivationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DurationInDays = table.Column<int>(type: "integer", nullable: false),
                    DefaultPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_CinemaHalls_CinemaHallId",
                        column: x => x.CinemaHallId,
                        principalTable: "CinemaHalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sessions_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PricePolicies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    PolicyStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsToEnd = table.Column<bool>(type: "boolean", nullable: false),
                    MovieSessionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricePolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricePolicies_Sessions_MovieSessionId",
                        column: x => x.MovieSessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieMovieGenre_MoviesId",
                table: "MovieMovieGenre",
                column: "MoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_PricePolicies_MovieSessionId",
                table: "PricePolicies",
                column: "MovieSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_CinemaHallId",
                table: "Sessions",
                column: "CinemaHallId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_MovieId",
                table: "Sessions",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieMovieGenre");

            migrationBuilder.DropTable(
                name: "PricePolicies");

            migrationBuilder.DropTable(
                name: "SalePolicies");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "CinemaHalls");

            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
