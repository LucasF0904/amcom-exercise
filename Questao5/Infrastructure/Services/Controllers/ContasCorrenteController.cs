using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Language;
using System;
using System.Threading.Tasks;

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContasCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContasCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{contaCorrenteId}/saldo")]
        public async Task<IActionResult> ConsultarSaldo(int contaCorrenteId)
        {
            try
            {
                var query = new ConsultarSaldoQuery { ContaCorrenteId = contaCorrenteId };
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
