using API1.Data;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly FilmContext _context;

        public FilmController(FilmContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Film>>> Get()
        {
            return Ok(await _context.Films.ToListAsync());
        }

        [HttpGet]
        [Route("{episodeId}")]
        public async Task<ActionResult<List<Film>>> Get(int episodeId)
        {
            try
            {
                var film = await _context.Films.Where(t => t.EpisodeId == episodeId).SingleOrDefaultAsync();
                var ships = await $"http://api2/api/Starship/{episodeId}".GetJsonAsync<List<Startship>>();
                var result = new FilmResult()
                {
                    Id = film.Id,
                    Director = film.Director,
                    EpisodeId = film.EpisodeId,
                    OpeningCrawl = film.OpeningCrawl,
                    Producer = film.Producer,
                    ReleaseDate = film.ReleaseDate,
                    Title = film.Title,
                    Starships = ships
                };

                return Ok(result);
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
    }
}