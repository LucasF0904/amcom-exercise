using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public class CriarMovimentacaoCommand : IRequest<CriarMovimentacaoResponse>
    {
        public string IdentificacaoRequisicao { get; set; }
        public int ContaCorrenteId { get; set; }
        public decimal Valor { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }
    }
}
