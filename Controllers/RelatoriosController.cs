using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FiapWebAluno.Data.Contexts;
using Esg.Horta.Entities; // <- contém Especie, Canteiro, SensorUmidade, Irrigacao
using FiapWebAluno.Models;

namespace FiapWebAluno.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly DatabaseContext _db;

        public RelatoriosController(DatabaseContext db)
        {
            _db = db;
        }

        // ===========================================================
        // ENDPOINT AVANÇADO 1 — RESUMO ESG DA HORTA
        // ===========================================================

        [HttpGet("resumo-horta")]
        [Authorize(Roles = "Gestor,Admin")]
        public async Task<IActionResult> GetResumoHortaAsync()
        {
            var totalCanteiros   = await _db.Set<Canteiro>().CountAsync();
            var totalEspecies    = await _db.Set<Especie>().CountAsync();
            var totalIrrigacoes  = await _db.Set<Irrigacao>().CountAsync();
            var totalSensores    = await _db.Set<SensorUmidade>().CountAsync();

            string nivelMonitoramento;

            if (totalCanteiros == 0)
            {
                nivelMonitoramento = "Sem canteiros cadastrados";
            }
            else
            {
                var relacao = (double)totalSensores / totalCanteiros;

                nivelMonitoramento = relacao switch
                {
                    < 0.5 => "Monitoramento baixo (poucos sensores por canteiro)",
                    < 1.5 => "Monitoramento adequado",
                    _     => "Monitoramento avançado (muitos sensores por canteiro)"
                };
            }

            return Ok(new
            {
                totalCanteiros,
                totalEspecies,
                totalIrrigacoes,
                totalSensores,
                nivelMonitoramento
            });
        }

        // ===========================================================
        // RECORD DO REQUEST — MODELO DE ENTRADA
        // ===========================================================

        public record PlanoIrrigacaoRequest(
            decimal AreaM2,
            decimal ConsumoLitrosPorM2Dia,
            int DiasPlanejados
        );

        // ===========================================================
        // ENDPOINT AVANÇADO 2 — PLANO DE IRRIGAÇÃO ESG
        // ===========================================================

        [HttpPost("plano-irrigacao-sugerido")]
        [Authorize(Roles = "Admin")]
        public IActionResult CalcularPlanoIrrigacao([FromBody] PlanoIrrigacaoRequest request)
        {
            if (request.AreaM2 <= 0 ||
                request.ConsumoLitrosPorM2Dia <= 0 ||
                request.DiasPlanejados <= 0)
            {
                return BadRequest("Parâmetros inválidos para cálculo do plano de irrigação.");
            }

            var consumoTotal = request.AreaM2 *
                               request.ConsumoLitrosPorM2Dia *
                               request.DiasPlanejados;

            var classificacao = consumoTotal switch
            {
                <= 500m  => "Baixo impacto hídrico (ótimo)",
                <= 2000m => "Consumo moderado (aceitável)",
                _        => "Consumo alto (atenção ESG — reveja o plano)"
            };

            return Ok(new
            {
                request.AreaM2,
                request.ConsumoLitrosPorM2Dia,
                request.DiasPlanejados,
                ConsumoTotalLitros = consumoTotal,
                Classificacao = classificacao
            });
        }
    }
}
