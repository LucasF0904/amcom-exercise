using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Language;
using System.Threading.Tasks;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaldosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SaldosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{idContaCorrente}")]
        public async Task<IActionResult> GetSaldo(string idContaCorrente)
        {
            var request = new GetSaldoContaCorrenteRequest { IdContaCorrente = idContaCorrente };
            var response = await _mediator.Send(request);

            if (!response.Success)
            {
                return BadRequest(new { response.Message, response.ErrorCode });
            }

            return Ok(response);
        }
    }
}
