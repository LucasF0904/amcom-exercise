using NSubstitute;
using Xunit;
using Questao5.Application.Handlers;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Database.CommandStore;
using System;
using System.Threading.Tasks;
using Questao5.Infrastructure.Database.Interfaces;
using Questao5.Domain.Enumerators;

namespace Questao5.Tests.Handlers
{
    public class CreateMovimentoHandlerTests
    {
        private readonly IContaCorrenteQueryStore _mockContaCorrenteQueryStore;
        private readonly IMovimentoCommandStore _mockMovimentoCommandStore;
        private readonly IIdempotenciaQueryStore _mockIdempotenciaQueryStore;

        public CreateMovimentoHandlerTests()
        {
            _mockContaCorrenteQueryStore = Substitute.For<IContaCorrenteQueryStore>();
            _mockMovimentoCommandStore = Substitute.For<IMovimentoCommandStore>();
            _mockIdempotenciaQueryStore = Substitute.For<IIdempotenciaQueryStore>();
        }

        private CreateMovimentoHandler CreateHandler()
        {
            return new CreateMovimentoHandler(
                _mockContaCorrenteQueryStore,
                _mockMovimentoCommandStore,
                _mockIdempotenciaQueryStore);
        }

        [Fact]
        public async Task HandleMovimento()
        {
            var requestId = Guid.NewGuid();
            string contaCorrenteId = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9"; 
            var valor = 100.00M; 
            var tipoMovimentacao = TipoMovimentacao.Credito;

            _mockContaCorrenteQueryStore.FindById(Arg.Any<string>()).Returns(new Questao5.Domain.Entities.ContaCorrente { Ativo = 1, IdContaCorrente = contaCorrenteId, Nome = "Eva Woodward", Numero = 123 });

            var request = new CreateMovimentoRequest
            {
                RequestId = requestId,
                ContaCorrenteId = contaCorrenteId,
                Valor = valor,
                TipoMovimentacao = tipoMovimentacao
            };

            var handler = CreateHandler();

            var result = await handler.Handle(request);

            Assert.NotNull(result);
            Assert.NotNull(result.Message);
            if(result.Success)
            {
                Assert.NotNull(result.MovimentoId);
            }
            else
            {
                Assert.NotNull(result.ErrorCode);
            }            
            
        }
    }
}
