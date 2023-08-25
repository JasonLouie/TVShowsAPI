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
    public class TVShowsController : ControllerBase
    {
        private readonly ShowsDBContext _context;

        public TVShowsController(ShowsDBContext context)
        {
            _context = context;
        }

        // GET: api/TVShows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TVShows>>> GetTVShows()
        {
            if (_context.TVShows == null){
                return NotFound(new Response(404, "any TV Shows. Table of TV Shows does not exist."));
            }

            var tvShows = await _context.TVShows.ToListAsync();

            // Stores genres for each tvShow for output tvShows.
            foreach (var tvShow in tvShows)
            {
                tvShow.Genres = await _context.Genres.Join(_context.ShowGenres.Where(s => s.ShowId == tvShow.ShowId), g => g.GenreId, s => s.GenreId, (genre, show) => new Genres
                {
                    GenreId = genre.GenreId,
                    Genre = genre.Genre
                }).ToListAsync();
            }

            if (tvShows.Count == 0)
            {
                return Ok(new Response(200, "any TV Shows. Database is empty."));
            }
            return Ok(new Response(200, "all TV Shows.", tvShows));
        }

        // GET: api/TVShows/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TVShows>> GetTVShows(int id)
        {
            if (_context.TVShows == null)
            {
                return NotFound(new Response(404, "any TV Shows. Table of TV Shows does not exist."));
            }

            var tVShow = await _context.TVShows.FindAsync(id);

            if (tVShow == null)
            {
                return NotFound(new Response(404, $"TV Show of id {id}. The TV Show does not exist in the database."));
            }

            // Stores genres of the tvShow for output.
            tVShow.Genres = await _context.Genres.Join(_context.ShowGenres.Where(s => s.ShowId == id), g => g.GenreId, s => s.GenreId, (genre, show) => new Genres
            {
                GenreId = genre.GenreId,
                Genre = genre.Genre
            }).ToListAsync();

            return Ok(new Response(200, $"TV Show of id {id}", tVShow));
        }

        // GET: api/TVShows/genre/{genre}
        [HttpGet("genre/{genre}")]
        public async Task<ActionResult<TVShows>> GetTVShows(string genre)
        {
            if (_context.TVShows == null)
            {
                return NotFound(new Response(404, "any TV Shows. Table of TV Shows does not exist."));
            }
            
            var results = await _context.TVShows.Join(_context.ShowGenres, t => t.ShowId, s => s.ShowId, (tvshow, showgenre) => new
            { tvshow, showgenre }).Join(_context.Genres.Where(g => g.Genre == genre), h => h.showgenre.GenreId, g => g.GenreId, (show, genre) => new TVShows
            {
                ShowId = show.tvshow.ShowId,
                ShowName = show.tvshow.ShowName,
                ShowDesc = show.tvshow.ShowDesc,
                Genres = _context.Genres.Join(_context.ShowGenres.Where(s => s.ShowId == show.tvshow.ShowId), g => g.GenreId, s => s.GenreId, (genre, show) => new Genres
                {
                    GenreId = genre.GenreId,
                    Genre = genre.Genre
                }).ToList(),
                NumSeasons = show.tvshow.NumSeasons,
                NumEpisodes = show.tvshow.NumEpisodes,
                EpisodeLength = show.tvshow.EpisodeLength,
                YearReleased = show.tvshow.YearReleased,
                Ongoing = show.tvshow.Ongoing,
                RTrating = show.tvshow.RTrating,
                IMDBrating = show.tvshow.IMDBrating,
                AVGUserRating = show.tvshow.AVGUserRating
            }).ToListAsync();

            if (results.Count == 0)
            {
                return NotFound(new Response(404, $"any TV Shows of genre {genre}."));
            }
            return Ok(new Response(200, $"{results.Count} TV Shows of genre {genre}. The genre specified does not exist in the database.", results));
        }

        // GET: api/TVShows/IMDB/top/5
        [HttpGet("{database}/{order}/{num}")]
        public async Task<ActionResult<TVShows>> GetTVShows(string database, string order, int num)
        {
            var results = new List<TVShows>();
            if (_context.TVShows == null)
            {
                return NotFound(new Response(404, "any TV Shows. Table of TV Shows does not exist."));
            }
            bool best = true;

            if (order.ToLower() == "worst")
            {
                best = false;
            }
            else if (order.ToLower() == "best" || order.ToLower() == "top")
            {
                best = true;
            }
            else
            {
                return NotFound(new Response(404, $"TV shows ranked by {order}. The order of ranking does not exist in the database."));
            }

            if (database == "RT" || database == "IMDB" || database == "DB")
            {
                switch (database.ToUpper())
                {
                    case "IMDB":
                        {
                            // Top {num} TV Shows based on IMDB rating
                            if (best)
                            {
                                results = await _context.TVShows.OrderByDescending(c => c.IMDBrating).Take(num).ToListAsync();
                                foreach (var result in results)
                                {
                                    result.Genres = await _context.Genres.Join(_context.ShowGenres.Where(s => s.ShowId == result.ShowId), g => g.GenreId, s => s.GenreId, (genre, show) => new Genres
                                    {
                                        GenreId = genre.GenreId,
                                        Genre = genre.Genre
                                    }).ToListAsync();
                                }
                                return Ok(new Response(200, $"the {num} {order} TV Shows from {database}", results));
                            }
                            // Worst {num} TV Shows based on IMDB rating
                            results = await _context.TVShows.OrderBy(c => c.IMDBrating).Take(num).ToListAsync();
                            foreach (var result in results)
                            {
                                result.Genres = await _context.Genres.Join(_context.ShowGenres.Where(s => s.ShowId == result.ShowId), g => g.GenreId, s => s.GenreId, (genre, show) => new Genres
                                {
                                    GenreId = genre.GenreId,
                                    Genre = genre.Genre
                                }).ToListAsync();
                            }
                            return Ok(new Response(200, $"the {num} {order} TV Shows from {database}", results));
                        }
                    case "RT":
                        {
                            // Top {num} TV Shows based on RT rating
                            if (best)
                            {
                                results = await _context.TVShows.OrderByDescending(c => c.RTrating).Take(num).ToListAsync();
                                foreach (var result in results)
                                {
                                    result.Genres = await _context.Genres.Join(_context.ShowGenres.Where(s => s.ShowId == result.ShowId), g => g.GenreId, s => s.GenreId, (genre, show) => new Genres
                                    {
                                        GenreId = genre.GenreId,
                                        Genre = genre.Genre
                                    }).ToListAsync();
                                }
                                return Ok(new Response(200, $"the {num} {order} TV Shows from {database}", results));
                            }
                            // Worst {num} TV Shows based on RT rating
                            results = await _context.TVShows.OrderBy(c => c.RTrating).Take(num).ToListAsync();
                            foreach (var result in results)
                            {
                                result.Genres = await _context.Genres.Join(_context.ShowGenres.Where(s => s.ShowId == result.ShowId), g => g.GenreId, s => s.GenreId, (genre, show) => new Genres
                                {
                                    GenreId = genre.GenreId,
                                    Genre = genre.Genre
                                }).ToListAsync();
                            }
                            return Ok(new Response(200, $"the {num} {order} TV Shows from {database}.", results));
                        }
                    case "DB":
                        {
                            // Top {num} TV Shows based on users' ratings from the database
                            if (best)
                            {
                                results = await _context.TVShows.OrderByDescending(c => c.AVGUserRating).Take(num).ToListAsync();
                                foreach (var result in results)
                                {
                                    result.Genres = await _context.Genres.Join(_context.ShowGenres.Where(s => s.ShowId == result.ShowId), g => g.GenreId, s => s.GenreId, (genre, show) => new Genres
                                    {
                                        GenreId = genre.GenreId,
                                        Genre = genre.Genre
                                    }).ToListAsync();
                                }
                                return Ok(new Response(200, $"the {num} {order} TV Shows from {database}.", results));
                            }
                            // Worst {num} TV Shows based on users' ratings from the database
                            results = await _context.TVShows.OrderBy(c => c.AVGUserRating).Take(num).ToListAsync();
                            foreach (var result in results)
                            {
                                result.Genres = await _context.Genres.Join(_context.ShowGenres.Where(s => s.ShowId == result.ShowId), g => g.GenreId, s => s.GenreId, (genre, show) => new Genres
                                {
                                    GenreId = genre.GenreId,
                                    Genre = genre.Genre
                                }).ToListAsync();
                            }
                            return Ok(new Response(200, $"the {num} {order} TV Shows from {database}.", results));
                        }
                    default:
                        return NotFound(new Response(404, $"{num} {order} TV Shows from {database}."));
                }
            }
            return NotFound(new Response(404, $"{num} {order} TV Shows from {database}."));
        }

        // PUT: api/TVShows/5
        // Returns Status Code 405 (Method Not Allowed)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTVShows(int id, TVShows tVShows)
        {
            /*
            if (id != tVShows.ShowId)
            {
                return BadRequest();
            }

            _context.Entry(tVShows).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TVShowsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
            */
            return StatusCode(405, new Response(405));
        }

        // POST: api/TVShows
        // Returns Status Code 405 (Method Not Allowed)
        [HttpPost]
        public async Task<ActionResult<TVShows>> PostTVShows(TVShows tVShows)
        {
            /*
              if (_context.TVShows == null)
              {
                  return Problem("Entity set 'ShowsDBContext.TVShows'  is null.");
              }
                _context.TVShows.Add(tVShows);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTVShows", new { id = tVShows.ShowId }, tVShows);
            */
            return StatusCode(405, new Response(405));
        }

        // DELETE: api/TVShows/5
        // Returns Status Code 405 (Method Not Allowed)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTVShows(int id)
        {
            /*
            if (_context.TVShows == null)
            {
                return NotFound();
            }
            var tVShows = await _context.TVShows.FindAsync(id);
            if (tVShows == null)
            {
                return NotFound();
            }

            _context.TVShows.Remove(tVShows);
            await _context.SaveChangesAsync();

            return NoContent();
            */
            return StatusCode(405, new Response(405));
        }

        private bool TVShowsExists(int id)
        {
            return (_context.TVShows?.Any(e => e.ShowId == id)).GetValueOrDefault();
        }
    }
}
