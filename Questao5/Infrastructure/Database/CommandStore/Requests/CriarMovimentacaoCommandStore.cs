using Microsoft.Data.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class CriarMovimentacaoCommandStore
    {
        private readonly string _connectionString;

        public CriarMovimentacaoCommandStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> CriarMovimentacaoAsync(int contaCorrenteId, decimal valor, string tipoMovimentacao)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"INSERT INTO Movimentacoes (ContaCorrenteId, Valor, TipoMovimentacao)
                        VALUES (@ContaCorrenteId, @Valor, @TipoMovimentacao);
                        SELECT last_insert_rowid();";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ContaCorrenteId", contaCorrenteId);
                    command.Parameters.AddWithValue("@Valor", valor);
                    command.Parameters.AddWithValue("@TipoMovimentacao", tipoMovimentacao);
                    var id = (long)await command.ExecuteScalarAsync();
                    return (int)id; // Retorna o ID da movimentação criada
                }
            }
        }
    }
}
