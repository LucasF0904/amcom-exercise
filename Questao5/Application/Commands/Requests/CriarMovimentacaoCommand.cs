using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class CriarMovimentacaoCommand : IRequest<CriarMovimentacaoResponse>
    {
        public int ContaCorrenteId { get; set; }
        public decimal Valor { get; set; }
        public char TipoMovimento { get; set; } 
    }
}
