using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class ConsultarSaldoQueryStore
    {
        private readonly string _connectionString;

        public ConsultarSaldoQueryStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<decimal> ConsultarSaldoAsync(int contaCorrenteId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"SELECT COALESCE(SUM(CASE WHEN TipoMovimentacao = 'C' THEN Valor ELSE -Valor END), 0) AS Saldo
                            FROM Movimentacoes
                            WHERE ContaCorrenteId = @ContaCorrenteId";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ContaCorrenteId", contaCorrenteId);
                    var resultado = await command.ExecuteScalarAsync();
                    var saldo = resultado != DBNull.Value ? Convert.ToDecimal(resultado) : 0m;

                    return saldo;
                }
            }
        }
    }
}
