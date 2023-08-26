using System;
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
    public class UsersController : ControllerBase
    {
        private readonly ShowsDBContext _context;

        public UsersController(ShowsDBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound(new Response(404, "any users. Table of users does not exist."));
            }
            var users = await _context.Users.ToListAsync();
            if (users.Count == 0)
            {
                return Ok(new Response(200, "There are not any users in the database."));
            }
            return Ok(new Response(200, "all users.", users));
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            if (_context.Users == null)
            {
                return NotFound(new Response(404, "any users. Table of users does not exist."));
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new Response(404, $"user of UserId {id}. User does not exist in the database."));
            }

            return Ok(new Response(200, $"user of UserId {id}.", user));
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, Users user)
        {
            if (id != user.UserId)
            {
                return BadRequest(new Response(400));
            }
            // Check to see if username is already being used in the database.
            if (UsersExists(0, user.Username))
            {
                return Conflict(new Response(409));
            }

            // Prevent Modification of UserId and NumOfReviews.
            _context.Entry(user).State = EntityState.Modified;
            _context.Entry(user).Property(x => x.UserId).IsModified = false;
            _context.Entry(user).Property(x => x.NumOfReviews).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
                {
                    return NotFound(new Response(404, $"user of UserId {id}. User does not exist in the database."));
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ShowsDBContext.Users'  is null.");
            }

            // Set UserId tor null so that user is given a UserId from the database.
            user.UserId = null;

            // Check to see if user id or username is already being used in the database.
            if (UsersExists(user.UserId, user.Username))
            {
                return Conflict(new Response(409));
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetUsers", new { id = user.UserId }, new Response(201, "user.", user));
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            if (_context.Users == null)
            {
                return NotFound(new Response(404, "any users. Table of users does not exist."));
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new Response(404, $"user of UserId {id}. User does not exist in the database."));
            }
            
            // Update AVGUserReview from TVShows table if deleted user had reviews.
            if (user.NumOfReviews > 0)
            {
                // Append ShowIds of user to a list. Used to find out which shows the user had a review for.
                var userreviews = await _context.UserReviews.Where(r => r.UserId == id).ToListAsync();
                List<int> showIds = new List<int>();
                foreach (var userreview in userreviews)
                {
                    showIds.Add(userreview.ShowId);
                }
                // Retrieve the tvShows that the user had a review for.
                var tvShows = await _context.TVShows.Where(t => showIds.Contains(t.ShowId)).ToListAsync();

                // Shouldn't ever have count == 0, but in case, check to see if user had any reviews based on list of ids.
                if (userreviews.Count > 0)
                {
                    // Update the AVGUserRating for each tvShow. If no other ratings exist, default the rating back to 0.
                    foreach (var tvShow in tvShows)
                    {
                        var reviews = await _context.UserReviews.Where(r => r.UserId != id).Where(r => r.ShowId == tvShow.ShowId).ToListAsync();
                        if (reviews.Count > 0)
                        {
                            tvShow.AVGUserRating = Math.Round(reviews.Average(r => r.UserRating), 1);
                        }
                        else
                        {
                            tvShow.AVGUserRating = 0;
                        }
                    }
                }
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersExists(int? id, string username = "")
        {
            return (_context.Users?.Any(e => e.UserId == id || e.Username == username)).GetValueOrDefault();
        }
    }
}
