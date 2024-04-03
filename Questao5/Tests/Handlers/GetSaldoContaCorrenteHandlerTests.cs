using NSubstitute;
using Xunit;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Database.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using System.Threading.Tasks;

namespace Questao5.Tests.Handlers
{
    public class GetSaldoContaCorrenteHandlerTests
    {
        private readonly IContaCorrenteQueryStore _mockContaCorrenteQueryStore;
        private readonly IMovimentoCommandStore _mockMovimentoQueryStore;

        public GetSaldoContaCorrenteHandlerTests()
        {
            _mockContaCorrenteQueryStore = Substitute.For<IContaCorrenteQueryStore>();
            _mockMovimentoQueryStore = Substitute.For<IMovimentoCommandStore>();
        }

        private GetSaldoContaCorrenteHandler CreateGetSaldoContaCorrenteHandler()
        {
            return new GetSaldoContaCorrenteHandler(
                _mockContaCorrenteQueryStore,
                _mockMovimentoQueryStore);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsExpectedSaldo()
        {
            var handler = CreateGetSaldoContaCorrenteHandler();
            string contaCorrenteId = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9"; 
            var request = new GetSaldoContaCorrenteRequest { IdContaCorrente = contaCorrenteId };
            var expectedSaldo = 100m; 
            _mockContaCorrenteQueryStore.FindById(Arg.Is(contaCorrenteId))
                .Returns(new ContaCorrente { IdContaCorrente = contaCorrenteId, Ativo = 1, Nome = "Teste", Numero = 123 });

            _mockMovimentoQueryStore.SumMovimentosByTipo(contaCorrenteId, TipoMovimentacao.Credito)
                .Returns(150m); 

            _mockMovimentoQueryStore.SumMovimentosByTipo(contaCorrenteId, TipoMovimentacao.Debito)
                .Returns(50m); 

            var result = await handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.Equal(expectedSaldo, result.Saldo);
            Assert.True(result.Success);
            _mockContaCorrenteQueryStore.Received(1).FindById(contaCorrenteId);
            _mockMovimentoQueryStore.Received(1).SumMovimentosByTipo(contaCorrenteId, TipoMovimentacao.Credito);
            _mockMovimentoQueryStore.Received(1).SumMovimentosByTipo(contaCorrenteId, TipoMovimentacao.Debito);
        }
    }
}
