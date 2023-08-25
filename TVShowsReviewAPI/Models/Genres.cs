using System;
using System.ComponentModel.DataAnnotations;
namespace TVShowsReviewAPI.Models
{
	public class Genres
	{
		[Key]
		public int GenreId { get; set; }
		public string Genre { get; set; }
	}
}
