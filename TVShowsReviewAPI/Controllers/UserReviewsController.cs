﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TVShowsReviewAPI.Models;

namespace TVShowsReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserReviewsController : ControllerBase
    {
        private readonly ShowsDBContext _context;

        public UserReviewsController(ShowsDBContext context)
        {
            _context = context;
        }

        // GET: api/UserReviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReviews>>> GetUserReviews()
        {
            if (_context.UserReviews == null)
            {
                return NotFound(new Response(404, "any user reviews. Table of user reviews does not exist."));
            }
            var userReviews = await _context.UserReviews.ToListAsync();
            if (userReviews.Count == 0)
            {
                return Ok(new Response(200, "There are not any user reviews in the database."));
            }
            return Ok(new Response(200, "all user reviews.", userReviews));
        }

        // Get based on ShowId or UserId (specified in type)
        // GET: api/UserReviews/shows/5
        [HttpGet("{type}/{id}")]
        public async Task<ActionResult<UserReviews>> GetUserReviews(string type, int id)
        {
            if (_context.UserReviews == null)
            {
                return NotFound(new Response(404, "any user reviews. Table of user reviews does not exist."));
            }
            if (id == null)
            {
                return BadRequest(new Response(400));
            }

            switch (type.ToLower())
            {
                case "shows":
                    {
                        var showReviews = await _context.UserReviews.Where(c => c.ShowId == id).ToListAsync();
                        if (showReviews != null && showReviews.Count > 0)
                        {
                            return Ok(new Response(200, $"all user reviews of ShowId {id}.", showReviews));
                        }
                        return Ok(new Response(200, $"There are not any user reviews for TV Show of ShowId {id}."));
                    }
                case "users":
                    {
                        var usersReviews = await _context.UserReviews.Where(c => c.UserId == id).ToListAsync();
                        if (usersReviews != null && usersReviews.Count > 0)
                        {
                            return Ok(new Response(200, $"all user reviews of UserId {id}.", usersReviews));
                        }
                        return Ok(new Response(200, $"There are not any user reviews for user of UserId {id}."));
                    }
                default:
                    return NotFound(new Response(404, $"any user reviews of type {type}."));
            }
        }

        // Get based on ReviewId
        // GET: api/UserReviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReviews>> GetUserReviews(int id)
        {
            if (_context.UserReviews == null)
            {
                return NotFound(new Response(404, "any user reviews. Table of user reviews does not exist."));
            }
            var userReviews = await _context.UserReviews.FindAsync(id);

            if (userReviews == null)
            {
                return NotFound(new Response(404, $"user review with review id of {id}. The review does not exist in the database."));
            }
            return Ok(new Response(200, "user reviews", userReviews));
        }

        // PUT: api/UserReviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserReviews(int id, UserReviews userReviews)
        {
            if (id != userReviews.ReviewId)
            {
                return BadRequest(new Response(400));
            }

            // Allow editing for a user review but prevent ShowId, ReviewId, and UserId from being modified.
            _context.Entry(userReviews).State = EntityState.Modified;
            _context.Entry(userReviews).Property(x => x.UserId).IsModified = false;
            _context.Entry(userReviews).Property(x => x.ReviewId).IsModified = false;
            _context.Entry(userReviews).Property(x => x.ShowId).IsModified = false;

            // Check if user rating was updated. Based on this update AVGUserRating of ShowId.
            if (_context.Entry(userReviews).Property(x => x.UserRating).IsModified == true)
            {
                var tvShow = await _context.TVShows.FindAsync(userReviews.ShowId);
                var reviews = await _context.UserReviews.Where(r => r.ShowId ==  userReviews.ShowId).ToListAsync();
                tvShow.AVGUserRating = Math.Round(reviews.Average(r => r.UserRating), 1);
            }

            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserReviewsExists(id))
                {
                    return NotFound(new Response(404, $"user review with review id of {id}. The review does not exist in the database."));
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserReviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserReviews>> PostUserReviews(UserReviews userReviews)
        {
            if (_context.UserReviews == null)
            {
                return Problem("Entity set 'ShowsDBContext.UserReviews'  is null.");
            }

            if (UserReviewsExists(userReviews.ReviewId, userReviews.ShowId, userReviews.UserId))
            {
                return Conflict(new Response(409));
            }

            var user = await _context.Users.FindAsync(userReviews.UserId);

            if (user == null)
            {
                return BadRequest(new Response(400));
            }

            // Set ReviewId to null so that user is given a ReviewId from database.
            userReviews.ReviewId = null;
            _context.UserReviews.Add(userReviews);
            user.NumOfReviews++;
            // Update AVGUserRating for that show
            var tvShow = await _context.TVShows.FindAsync(userReviews.ShowId);
            var reviews = await _context.UserReviews.Where(r => r.ShowId == userReviews.ShowId).ToListAsync();

            // The new user review does not register as added when recalculating using Average() so it must be manually done like this.
            tvShow.AVGUserRating = Math.Round((reviews.Sum(r => r.UserRating)+userReviews.UserRating) / (reviews.Count+1), 1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserReviews", new { id = userReviews.ReviewId }, new Response(201, $"user review for user of UserId {userReviews.UserId} for ShowId {userReviews.ShowId}.", userReviews));
        }

        // DELETE: api/UserReviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserReviews(int id)
        {
            if (_context.UserReviews == null)
            {
                return NotFound(new Response(404, "any user reviews. Table of user reviews does not exist."));
            }
            var userReviews = await _context.UserReviews.FindAsync(id);
            if (userReviews == null)
            {
                return NotFound(new Response(404, $"user review with review id of {id}. The review does not exist in the database."));
            }

            var user = await _context.Users.FindAsync(userReviews.UserId);

            if (user == null)
            {
                return BadRequest(new Response(400));
            }

            var tvShow = await _context.TVShows.FindAsync(userReviews.ShowId);
            var reviews = await _context.UserReviews.Where(r => r.ShowId == userReviews.ShowId).Where(r => r.ReviewId != userReviews.ReviewId).ToListAsync();
            
            // Check if there are other UserReviews for the show. If there are, update the show.
            if (reviews.Count > 0)
            {
                tvShow.AVGUserRating = Math.Round(reviews.Average(r => r.UserRating), 1);
            }

            // There aren't any other UserReviews for the show.
            else
            {
                tvShow.AVGUserRating = 0;
            }
            user.NumOfReviews--;

            _context.UserReviews.Remove(userReviews);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserReviewsExists(int? rid = 0, int sid = 0, int uid = 0)
        {
            return (_context.UserReviews?.Any(e => e.ReviewId == rid || (e.ShowId == sid && e.UserId == uid))).GetValueOrDefault();
        }
    }
}
