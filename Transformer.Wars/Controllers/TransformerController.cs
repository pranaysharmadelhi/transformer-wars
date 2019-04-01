using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using Transformer.Wars.Exceptions;
using Transformer.Wars.Interfaces;
using Transformer.Wars.Models.DB;
using Transformer.Wars.Models.DTO;
using Transformer.Wars.Services;

namespace Transformer.Wars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransformerController : ControllerBase
    {
        private readonly IWarService _warService;
        private ILogger _logger;

        public TransformerController(IWarService warService, ILogger<TransformerController> logger)
        {
            _warService = warService;
            _logger = logger;
        }

        [HttpPut()]
        [SwaggerOperation("Add a new Transformer")]
        public async Task<ActionResult<Models.DB.Transformer>> Put([FromBody] TransformerRequest transformer)
        {
            try
            {
                return await _warService.AddTransformer(transformer);
            }
            catch (TransformerWarsException ex)
            {
                _logger.LogError(ex, "Error Adding Transformer");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Retrieve the details of a registered Transformer")]
        public async Task<ActionResult<Models.DB.Transformer>> Get(int id)
        {
            try
            {
                return await _warService.GetTransformer(id);
            }
            catch (TransformerWarsException ex)
            {
                _logger.LogError(ex, "Error Getting Transformer#" + id);
                return NotFound();
            }
        }

        [HttpGet("{id}/score")]
        [SwaggerOperation("Retrieve score of a registered Transformer")]
        public async Task<ActionResult<int>> GetScore(int id)
        {
            try
            {
                return await _warService.GetScore(id);
            }
            catch (TransformerWarsException ex)
            {
                _logger.LogError(ex, "Error Getting Transformer#" + id);
                return NotFound();
            }
        }

        [HttpPost("{id}")]
        [SwaggerOperation("Update a Transformer")]
        public async Task<ActionResult<Models.DB.Transformer>> Post([FromRoute]int id, [FromBody] TransformerRequest transformer)
        {
            try
            {
                return await _warService.UpdateTransformer(id, transformer);
            }
            catch (TransformerWarsException ex)
            {
                _logger.LogError(ex, "Error Getting Transformer#" + id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Remove a registered Transformer")]
        public async Task Delete([FromRoute]int id)
        {
            await _warService.DeleteTransformer(id);
        }

        [HttpGet()]
        [SwaggerOperation("Get a list of all the registered Autobots or Decepticons")]
        public async Task<ActionResult<IEnumerable<Models.DB.Transformer>>> GetTeam([FromQuery]Models.AllegianceTypes allegiance)
        {
            return await _warService.GetTransformers(allegiance);
        }


    }
}
