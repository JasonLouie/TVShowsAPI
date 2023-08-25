using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.CodeAnalysis;
using TVShowsReviewAPI.Models;

namespace TVShowsReviewAPI.Models
{

	public class ShowsDBContext : DbContext
    {
		protected readonly IConfiguration Configuration;

        public ShowsDBContext(DbContextOptions<ShowsDBContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = Configuration.GetConnectionString("TVShowsDatabase");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        public DbSet<TVShows> TVShows { get; set; } = null!;
        public DbSet<UserReviews> UserReviews { get; set; } = null!;
        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<Genres> Genres { get; set; } = null!;
        public DbSet<ShowGenres> ShowGenres { get; set; } = null!;
    }
}
