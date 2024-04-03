using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Handlers;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovimentacoesController : ControllerBase
    {
        private readonly CreateMovimentoHandler _createMovimentoHandler;

        public MovimentacoesController(CreateMovimentoHandler createMovimentoHandler)
        {
            _createMovimentoHandler = createMovimentoHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovimento([FromBody] CreateMovimentoRequest request)
        {
            var response = await _createMovimentoHandler.Handle(request);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.MovimentoId);
        }
    }
}
