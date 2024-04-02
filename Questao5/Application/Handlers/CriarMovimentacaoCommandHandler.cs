using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Handlers
{
    public class CriarMovimentacaoCommandHandler : IRequestHandler<CriarMovimentacaoCommand, CriarMovimentacaoResponse>
    {

        public async Task<CriarMovimentacaoResponse> Handle(CriarMovimentacaoCommand request, CancellationToken cancellationToken)
        {
            return new CriarMovimentacaoResponse
            {
                MovimentacaoId = 0
            };
        }
    }
}

