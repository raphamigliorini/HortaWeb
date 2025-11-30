using FiapWebAluno.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using FiapWebAluno.Views.Canteiro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CanteirosController : ControllerBase
    {
        private readonly ICanteiroService _service;

        public CanteirosController(ICanteiroService service)
        {
            _service = service;
        }

        // GET PAGINADO
        
        [HttpGet]
        public async Task<IActionResult> Get(
            int page = 1,
            int pageSize = 10)
        {
            var result = await _service.ListarAsync(page, pageSize);
            return Ok(result);
        }

        // POST (CRIAR)

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CanteiroCreateView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CriarAsync(model);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
    }
}
