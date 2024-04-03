using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Interfaces;
using System.Data;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public class ContaCorrenteQueryStore : IContaCorrenteQueryStore
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

        public ContaCorrente FindById(string idContaCorrente)
        {
            using (var connection = new SqliteConnection(GetDatabaseConnectionString()))
            {
                connection.Open();
                var query = "SELECT * FROM ContaCorrente WHERE idcontacorrente = @idContaCorrente";
                try
                {
                    return connection.QueryFirstOrDefault<ContaCorrente>(query, new { idContaCorrente = idContaCorrente });
                }
                catch (SqliteException)
                {
                    return null;
                }
            }            
        }

        public async Task<ContaCorrente> FindByIdAsync(string idContaCorrente)
        {
            using (var connection = new SqliteConnection(GetDatabaseConnectionString()))
            {
                connection.Open();
                var query = "SELECT * FROM ContaCorrente WHERE IdContaCorrente = @Id";
                return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(query, new { Id = idContaCorrente });
            }           
        }

        public decimal GetSaldoAtual(string idContaCorrente)
        {
            using (var connection = new SqliteConnection(GetDatabaseConnectionString()))
            {
                connection.Open();
                var query = @"SELECT SUM(Valor) FROM Movimento
                              WHERE IdContaCorrente = @IdContaCorrente";
                return connection.QuerySingle<decimal>(query, new { IdContaCorrente = idContaCorrente });
            }
            
        }

        public bool IsContaAtivo(string idContaCorrente)
        {
            using (var connection = new SqliteConnection(GetDatabaseConnectionString()))
            {
                connection.Open();
                var query = "SELECT ativo FROM contacorrente WHERE idcontacorrente = @idContaCorrente";
                try
                {
                    int ativo = connection.QueryFirstOrDefault<int>(query, new { idContaCorrente = idContaCorrente });
                    return ativo == 1;
                }
                catch (SqliteException)
                {
                    return false;
                }
            }
        }


        public async Task<bool> IsContaAtivoAsync(string idContaCorrente)
        {
            using (var connection = new SqliteConnection(GetDatabaseConnectionString()))
            {
                connection.Open();
                var query = "SELECT ativo FROM contacorrente WHERE idcontacorrente = @IdContaCorrente";
                return await connection.QueryFirstOrDefaultAsync<bool>(query, new { IdContaCorrente = idContaCorrente });
            }
        }
    }

}
