using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Services.Controllers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Questao5.Domain.Enumerators; // Supondo que este seja o namespace para TipoMovimentacao

namespace Questao5.Tests.Infrastructure.Controller
{
    public class MovimentacoesControllerTests
    {
        private readonly Mock<IMediator> mockMediator;

        public MovimentacoesControllerTests()
        {
            mockMediator = new Mock<IMediator>(MockBehavior.Strict);
        }

        private MovimentacoesController CreateMovimentacoesController()
        {
            return new MovimentacoesController(mockMediator.Object);
        }

        [Fact]
        public async Task CriarMovimentacao_ReturnsOkResult_WithValidCommand()
        {
            // Arrange
            var movimentacoesController = CreateMovimentacoesController();
            var command = new CriarMovimentacaoCommand
            {
                IdentificacaoRequisicao = "unique-id-123", 
                ContaCorrenteId = 1,
                Valor = 100.0m,
                TipoMovimentacao = TipoMovimentacao.Credito
            };
            var expectedResponse = new CriarMovimentacaoResponse
            {
                MovimentacaoId = 42 
            };

            mockMediator
                .Setup(m => m.Send(It.Is<CriarMovimentacaoCommand>(cmd => cmd == command), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse)
                .Verifiable("O comando não foi enviado corretamente.");

            // Act
            var result = await movimentacoesController.CriarMovimentacao(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CriarMovimentacaoResponse>(okResult.Value);
            Assert.Equal(expectedResponse.MovimentacaoId, returnValue.MovimentacaoId);

            mockMediator.Verify(m => m.Send(It.Is<CriarMovimentacaoCommand>(cmd => cmd == command), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
