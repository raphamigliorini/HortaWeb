using FiapWebAluno.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using FiapWebAluno.Views.Especie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class EspecieController : ControllerBase
    {
        private readonly IEspecieService _service;

        public EspecieController(IEspecieService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10)
        {
            return Ok(await _service.ListarAsync(page, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EspecieCreateView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CriarAsync(model);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
    }
}
