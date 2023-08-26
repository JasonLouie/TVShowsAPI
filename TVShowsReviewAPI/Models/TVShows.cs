using System;
using System.ComponentModel.DataAnnotations;
namespace TVShowsReviewAPI.Models
{
	public class TVShows
	{
        [Key]
        public int ShowId { get; set; }
		public string ShowName { get; set; }
		public string ShowDesc { get; set; }
		public virtual ICollection<Genres> Genres { get; set; } = new List<Genres>();
		public int NumSeasons { get; set; }
		public int NumEpisodes { get; set;}
		public int EpisodeLength { get; set; }
		public int YearReleased { get; set; }
		public bool Ongoing { get; set; }
		public double RTrating { get; set; }
		public double IMDBrating { get; set; }
		public double AVGUserRating { get; set; }
	}
}
