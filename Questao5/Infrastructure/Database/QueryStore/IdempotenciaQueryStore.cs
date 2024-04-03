using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Interfaces;
using System.Data;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public class IdempotenciaQueryStore : IIdempotenciaQueryStore
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

        public async Task<Idempotencia> FindByIdAsync(Guid chaveIdempotencia)
        {
            using (var connection = new SqliteConnection(GetDatabaseConnectionString()))
            {
                connection.Open();
                var query = "SELECT * FROM Idempotencia WHERE ChaveIdempotencia = @ChaveIdempotencia";
                return await connection.QueryFirstOrDefaultAsync<Idempotencia>(query, chaveIdempotencia);
            }           
        }

        public async Task SaveAsync(Idempotencia idempotencia)
        {
            using (var connection = new SqliteConnection(GetDatabaseConnectionString()))
            {
                connection.Open();
                var query = @"INSERT INTO Idempotencia (ChaveIdempotencia, IdMovimento)
                              VALUES (@ChaveIdempotencia, @IdMovimento)";
                await connection.ExecuteAsync(query, idempotencia);
            }                        
        }

        public Idempotencia FindById(Guid chaveIdempotencia)
        {
            using (var connection = new SqliteConnection(GetDatabaseConnectionString()))
            {
                connection.Open();
                var query = "SELECT * FROM idempotencia WHERE ChaveIdempotencia = @ChaveIdempotencia";

                try
                {
                    return connection.QueryFirstOrDefault<Idempotencia>(query, new { ChaveIdempotencia = chaveIdempotencia });
                }
                catch (SqliteException)
                {
                    return null;
                }
            }
        }


        public void Save(Idempotencia idempotencia)
        {
            string newGuidString = Guid.NewGuid().ToString();

            using (var connection = new SqliteConnection(GetDatabaseConnectionString()))
            {
                connection.Open();
                var insertquery = @"INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                              VALUES (@chave_idempotencia, @requisicao, @resultado)";

                connection.Execute(insertquery, new
                {
                    chave_idempotencia = newGuidString,
                    requisicao = idempotencia.Requisicao,
                    resultado = idempotencia.Resultado
                });
            }
        }
    }

}
