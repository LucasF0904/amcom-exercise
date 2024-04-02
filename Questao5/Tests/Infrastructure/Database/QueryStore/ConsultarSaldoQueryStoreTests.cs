using Microsoft.Data.Sqlite;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Tests
{
    public class ConsultarSaldoQueryStoreTests
    {
        private readonly ConsultarSaldoQueryStore queryStore;

        public ConsultarSaldoQueryStoreTests()
        {
            var databaseConfig = new DatabaseConfig { Name = "Data Source=database.sqlite" };

            queryStore = new ConsultarSaldoQueryStore(databaseConfig.Name);
        }

        [Fact]
        public async Task ConsultarSaldoAsync_ReturnsCorrectBalance()
        {
            var contaCorrenteId = 1;

            var saldo = await queryStore.ConsultarSaldoAsync(contaCorrenteId);

            var saldoEsperado = 100m;
            Assert.Equal(saldoEsperado, saldo);
        }
    }
}
