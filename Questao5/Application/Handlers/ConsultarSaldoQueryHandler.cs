using MediatR;
using Questao5.Application.Queries;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Questao5.Application.Handlers
{
    public class ConsultarSaldoQueryHandler : IRequestHandler<ConsultarSaldoQuery, ConsultarSaldoResponse>
    {
        // Injeção de dependências

        public ConsultarSaldoQueryHandler(/* Dependências */)
        {
            // Inicializar dependências
        }


        Task<ConsultarSaldoResponse> IRequestHandler<ConsultarSaldoQuery, ConsultarSaldoResponse>.Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
