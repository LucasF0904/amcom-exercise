using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Sqlite; // Suponha que este seja o namespace correto para DatabaseConfig
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Questao5.Application.Handlers
{
    public class ConsultarSaldoQueryHandler : IRequestHandler<ConsultarSaldoQuery, ConsultarSaldoResponse>
    {
        private readonly DatabaseConfig _databaseConfig;

        public ConsultarSaldoQueryHandler(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<ConsultarSaldoResponse> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
        {
            var resultado = await ConsultarSaldoAsync(request.ContaCorrenteId, cancellationToken);
            if (!resultado.IsValid)
            {
                throw new Exception(resultado.ErrorMessage);
            }

            return resultado.Response!;
        }

        private async Task<(bool IsValid, string ErrorMessage, ConsultarSaldoResponse? Response)> ConsultarSaldoAsync(int contaCorrenteId, CancellationToken cancellationToken)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.OpenAsync(cancellationToken);

            var contaCmd = connection.CreateCommand();
            contaCmd.CommandText = @"SELECT numero, nome, ativo FROM contacorrente WHERE idcontacorrente = @Id";
            contaCmd.Parameters.AddWithValue("@Id", contaCorrenteId);

            var reader = await contaCmd.ExecuteReaderAsync(cancellationToken);
            if (!await reader.ReadAsync(cancellationToken))
            {
                return (false, "A conta corrente especificada não existe.", null);
            }

            var ativo = reader.GetBoolean(reader.GetOrdinal("ativo"));
            if (!ativo)
            {
                return (false, "A conta corrente especificada está inativa.", null);
            }

            reader.Close();

            var saldoCmd = connection.CreateCommand();
            saldoCmd.CommandText = @"SELECT COALESCE(SUM(CASE WHEN TipoMovimentacao = 'C' THEN Valor ELSE -Valor END), 0) AS Saldo FROM movimento WHERE idcontacorrente = @Id";
            saldoCmd.Parameters.AddWithValue("@Id", contaCorrenteId);

            var saldo = (decimal)await saldoCmd.ExecuteScalarAsync(cancellationToken);

            return (true, "", new ConsultarSaldoResponse
            {
                Saldo = saldo
            });
        }
    }
}
