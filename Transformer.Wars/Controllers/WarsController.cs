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
    public class WarsController : ControllerBase
    {
        private readonly IWarService _warService;
        private ILogger _logger;

        public WarsController(IWarService warService, ILogger<TransformerController> logger)
        {
            _warService = warService;
            _logger = logger;
        }

        [HttpDelete()]
        [SwaggerOperation("Tactical Nuke")]
        public async Task Delete()
        {
            await _warService.TacticalNuke();
        }

        [HttpGet()]
        [SwaggerOperation("Run the war")]
        public async Task<ActionResult<Models.DTO.WarsResponse>> Get()
        {
            return await _warService.RunWar();
        }


    }
}
