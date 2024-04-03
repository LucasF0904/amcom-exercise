using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Questao5.Application.Handlers
{
    public class GetSaldoContaCorrenteHandler : IRequestHandler<GetSaldoContaCorrenteRequest, GetSaldoContaCorrenteResponse>
    {
        private readonly IContaCorrenteQueryStore _contaCorrenteQueryStore;
        private readonly IMovimentoCommandStore _movimentoQueryStore;

        public GetSaldoContaCorrenteHandler(IContaCorrenteQueryStore contaCorrenteQueryStore, IMovimentoCommandStore movimentoQueryStore)
        {
            _contaCorrenteQueryStore = contaCorrenteQueryStore;
            _movimentoQueryStore = movimentoQueryStore;
        }

        public async Task<GetSaldoContaCorrenteResponse> Handle(GetSaldoContaCorrenteRequest request, CancellationToken cancellationToken)
        {
            var contaCorrente = _contaCorrenteQueryStore.FindById(request.IdContaCorrente);
            if (contaCorrente == null)
            {
                return new GetSaldoContaCorrenteResponse
                {
                    Success = false,
                    Message = "Conta corrente não encontrada.",
                    ErrorCode = Messages.INVALID_ACCOUNT_ID
                };
            }

            if (contaCorrente.Ativo != 1)
            {
                return new GetSaldoContaCorrenteResponse
                {
                    Success = false,
                    Message = "Conta corrente inativa.",
                    ErrorCode = Messages.INVALID_ACCOUNT
                };
            }

            var creditos = _movimentoQueryStore.SumMovimentosByTipo(request.IdContaCorrente, TipoMovimentacao.Credito);
            var debitos = _movimentoQueryStore.SumMovimentosByTipo(request.IdContaCorrente, TipoMovimentacao.Debito);

            var saldo = creditos - debitos;

            return new GetSaldoContaCorrenteResponse
            {
                Success = true,
                Message = "Saldo consultado com sucesso.",
                NumeroContaCorrente = contaCorrente.Numero.ToString(),
                NomeTitular = contaCorrente.Nome,
                DataHoraConsulta = DateTime.UtcNow,
                Saldo = saldo
            };
        }
    }
}
