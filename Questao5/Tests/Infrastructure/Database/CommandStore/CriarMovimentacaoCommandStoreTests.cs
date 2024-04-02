using Microsoft.Data.Sqlite;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Sqlite; // Garanta que este namespace seja correto

namespace Questao5.Tests
{
    public class CriarMovimentacaoCommandStoreTests
    {
        [Fact]
        public async Task CriarMovimentacaoAsync_ReturnsNewId()
        {
            var databaseConfig = new DatabaseConfig { Name = "Data Source=database.sqlite" };

            var commandStore = new CriarMovimentacaoCommandStore(databaseConfig.Name);

            var contaCorrenteId = 1;
            var valor = 100.0m;
            var tipoMovimentacao = TipoMovimentacao.Credito;

            var resultado = await commandStore.CriarMovimentacaoAsync(contaCorrenteId, valor, tipoMovimentacao);

            Assert.True(resultado > 0, "O ID da movimentação criada deve ser maior que 0.");
        }
    }
}
