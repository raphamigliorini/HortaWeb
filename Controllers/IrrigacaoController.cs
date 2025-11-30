using FiapWebAluno.Service.Interface;
using FiapWebAluno.Views.Irrigacao;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapWebAluno.Controllers
{
    public class IrrigacaoController : ControllerBase
    {
        private readonly IIrrigacaoService _service;

        public IrrigacaoController(IIrrigacaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10)
        {
            return Ok(await _service.ListarAsync(page, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IrrigacaoCreateView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _service.CriarAsync(model);
            return CreatedAtAction(nameof(Get), new { id = result.IdIrrigacao }, result);
        }
    }
}
