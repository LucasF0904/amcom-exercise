using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public class CreateMovimentoRequest : IRequest<CreateMovimentoResponse>
    {
        public Guid RequestId { get; set; } 
        public string ContaCorrenteId { get; set; }
        public decimal Valor { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }
    }
}
