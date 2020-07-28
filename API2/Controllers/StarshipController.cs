using Elastic.Apm;
using Elastic.Apm.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<StarshipController> _logger;

        public StarshipController(IAsyncDocumentSession db, ILogger<StarshipController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Starship>>> Get()
        {
            var result = new List<Starship>();
            await Agent.Tracer.CaptureTransaction("GetStarships", ApiConstants.TypeRequest,
            async (transaction) =>
            {
                transaction.Labels["LabelTeste"] = "foo";

                result = await _db.Query<Starship>().ToListAsync();

                transaction.End();

            });
            return Ok(result);
        }

        [HttpGet]
        [Route("{filmId}")]
        public async Task<ActionResult<List<Starship>>> Get(string filmId)
        {
            _logger.LogInformation($"Get for film {filmId}");
            return Ok(await _db.Query<Starship>().Where(t => t.Films.Contains(filmId)).ToListAsync());
        }
    }
}