using ConsultaAtivos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaAtivos.API.Controllers
{
    // AtivosController.cs
    [ApiController]
    [Route("api/ativos")]
    public class AtivosController : ControllerBase
    {
        private readonly IConsultaAtivosService _consultaAtivosService;

        public AtivosController(IConsultaAtivosService consultaAtivosService)
        {
            _consultaAtivosService = consultaAtivosService;
        }

        /// <summary>
        /// Retorna resumo da cotação de um ativo a partir do ticker informado.
        /// </summary>
        /// <param name="symbol">Código do ativo (ex: MGLU3)</param>
        /// <returns>Resumo da cotação</returns>
        [HttpGet("resumo")]
        public async Task<IActionResult> ObterResumo([FromQuery] string symbol, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                return BadRequest("Símbolo do ativo é obrigatório.");

            try
            {
                var resumo = await _consultaAtivosService.ObterResumoAsync(symbol, cancellationToken);
                return Ok(resumo);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }

}
