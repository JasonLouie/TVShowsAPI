using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TVShowsReviewAPI.Models
{
	public class UserReviews
	{
		[Key]
		public int? ReviewId { get; set; }
		public int ShowId { get; set; }
		public int UserId { get; set; }
		public double UserRating { get; set; }
        public string? UserComment { get; set; }
	}
}
