using ConsultaAtivos.Application.Services;
using ConsultaAtivos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaAtivos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoricalQuotesController : ControllerBase
    {
        private readonly ConsultaHistoricalQuoteService _service;

        public HistoricalQuotesController(ConsultaHistoricalQuoteService service)
        {
            _service = service;
        }

        [HttpGet("{ticker}")]
        public async Task<ActionResult<IEnumerable<HistoricalQuote>>> Get(string ticker, DateTime inicio, DateTime fim)
        {
            try
            {
                var quotes = await _service.ConsultarAsync(ticker, inicio, fim);
                return Ok(quotes);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro inesperado: {ex.Message}");
            }
        }
    }
}
