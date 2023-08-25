using System;
using System.ComponentModel.DataAnnotations;
namespace TVShowsReviewAPI.Models
{
	public class ShowGenres
	{
		[Key]
		public int ShowId { get; set; }
		public int GenreId { get; set; }
	}
}
