using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StarshipController : ControllerBase
    {
        private readonly IAsyncDocumentSession _db;

        public StarshipController(IAsyncDocumentSession db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Startship>>> Get()
        {
            return Ok(await _db.Query<Startship>().ToListAsync());
        }

        [HttpGet]
        [Route("{filmId}")]
        public async Task<ActionResult<List<Startship>>> Get(string filmId)
        {
            return Ok(await _db.Query<Startship>().Where(t => t.Films.Contains(filmId)).ToListAsync());
        }
    }
}