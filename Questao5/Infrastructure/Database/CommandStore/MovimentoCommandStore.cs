using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.Interfaces;
using System.Data;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class MovimentoCommandStore : IMovimentoCommandStore
    {

        private string GetDatabaseConnectionString()
        {
            var dbFilePath = Environment.GetEnvironmentVariable("DATABASE_PATH");
            if (string.IsNullOrEmpty(dbFilePath))
            {
                throw new InvalidOperationException("DATABASE_PATH environment variable is not set.");
            }
            return $"Data Source={dbFilePath}";
        }

        public string CreateMovimento(Movimento movimento)
        {
            string newGuidString = Guid.NewGuid().ToString();
            string dataMovimentoFormatada = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            using (var connection = new SqliteConnection(GetDatabaseConnectionString()))
            {
                connection.Open();

                var insertQuery = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                            VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);";
                connection.Execute(insertQuery, new
                {
                    IdMovimento = newGuidString,
                    IdContaCorrente = movimento.IdContaCorrente,
                    DataMovimento = dataMovimentoFormatada,
                    TipoMovimento = movimento.TipoMovimento.GetEnumCharValue(),
                    Valor = movimento.Valor
                });

                var idQuery = @"SELECT idmovimento FROM movimento ORDER BY ROWID DESC LIMIT 1;";
                string idMovimento = connection.QuerySingle<string>(idQuery);

                return idMovimento;
            }
        }

        public void DeleteMovimento(Guid idMovimento)
        {
            using (var connection = new SqliteConnection(GetDatabaseConnectionString()))
            {
                connection.Open();
                var query = "DELETE FROM Movimento WHERE IdMovimento = @IdMovimento";
                connection.Execute(query, new { IdMovimento = idMovimento });
            }            
        }

        public decimal SumMovimentosByTipo(string idContaCorrente, TipoMovimentacao tipoMovimento)
        {
            using (var connection = new SqliteConnection(GetDatabaseConnectionString()))
            {
                connection.Open();
                try
                {
                    var query = @"SELECT IFNULL(SUM(Valor), 0) FROM movimento
                      WHERE idcontacorrente = @IdContaCorrente AND tipomovimento = @TipoMovimento";
                    char tipoMovimentoChar = tipoMovimento.GetEnumCharValue();
                    return connection.QuerySingle<decimal>(query, new { IdContaCorrente = idContaCorrente, TipoMovimento = tipoMovimentoChar.ToString() });
                }
                catch (SqliteException)
                {
                    return 0;
                }
            }
        }

        public void UpdateMovimento(Movimento movimento)
        {            
            using (var connection = new SqliteConnection(GetDatabaseConnectionString()))
            {
                connection.Open();
                var query = @"UPDATE Movimento
                              SET IdContaCorrente = @IdContaCorrente, TipoMovimento = @TipoMovimento, Valor = @Valor, DataHora = @DataHora
                              WHERE IdMovimento = @IdMovimento";
                connection.Execute(query, movimento);
            }
        }
    }

}
