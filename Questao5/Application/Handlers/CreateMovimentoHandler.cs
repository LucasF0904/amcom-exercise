using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Questao5.Application.Handlers
{
    public class CreateMovimentoHandler
    {
        private readonly IContaCorrenteQueryStore _contaCorrenteQueryStore;
        private readonly IMovimentoCommandStore _movimentoCommandStore;
        private readonly IIdempotenciaQueryStore _idempotenciaQueryStore;

        public CreateMovimentoHandler(IContaCorrenteQueryStore contaCorrenteQueryStore, IMovimentoCommandStore movimentoCommandStore, IIdempotenciaQueryStore idempotenciaQueryStore)
        {
            _contaCorrenteQueryStore = contaCorrenteQueryStore;
            _movimentoCommandStore = movimentoCommandStore;
            _idempotenciaQueryStore = idempotenciaQueryStore;
        }

        public async Task<CreateMovimentoResponse> Handle(CreateMovimentoRequest request)
        {
            var idempotencia = _idempotenciaQueryStore.FindById(request.RequestId);
            if (idempotencia != null)
            {
                return new CreateMovimentoResponse
                {
                    Success = true,
                    MovimentoId = new Guid(idempotencia.Resultado).ToString()
                };
            }

            var contaCorrenteAtiva = _contaCorrenteQueryStore.IsContaAtivo(request.ContaCorrenteId);
            if (!contaCorrenteAtiva)
            {
                return new CreateMovimentoResponse
                {
                    Success = false,
                    Message = "Conta corrente não cadastrada ou inativa.",
                    ErrorCode = Messages.INVALID_ACCOUNT
                };
            }

            if (request.Valor <= 0)
            {
                return new CreateMovimentoResponse
                {
                    Success = false,
                    Message = "O valor deve ser positivo.",
                    ErrorCode = Messages.INVALID_AMOUNT
                };
            }

            if (!Enum.IsDefined(typeof(TipoMovimentacao), request.TipoMovimentacao))
            {
                return new CreateMovimentoResponse
                {
                    Success = false,
                    Message = "Tipo de movimento inválido.",
                    ErrorCode = Messages.INVALID_MOVEMENT_TYPE
                };
            }



            var movimentoId = _movimentoCommandStore.CreateMovimento(new Movimento
            {
                IdContaCorrente = request.ContaCorrenteId,
                DataMovimento = DateTime.UtcNow.ToString(), 
                TipoMovimento = request.TipoMovimentacao,
                Valor = request.Valor
            });

            _idempotenciaQueryStore.Save(new Idempotencia
            {
                Requisicao = request.ToString(), 
                Resultado = movimentoId.ToString()
            });

            return new CreateMovimentoResponse
            {
                Success = true,
                MovimentoId = movimentoId.ToString()
            };
        }
    }
}
