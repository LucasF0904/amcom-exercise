using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Sqlite; // Ajuste para o namespace correto da sua DatabaseConfig
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Questao5.Application.Handlers
{
    public class CriarMovimentacaoCommandHandler : IRequestHandler<CriarMovimentacaoCommand, CriarMovimentacaoResponse>
    {
        private readonly DatabaseConfig _databaseConfig;

        public CriarMovimentacaoCommandHandler(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<CriarMovimentacaoResponse> Handle(CriarMovimentacaoCommand request, CancellationToken cancellationToken)
        {
            if (request.Valor <= 0) throw new ArgumentException(Messages.ValorInvalido);

            using (var connection = new SqliteConnection(_databaseConfig.Name))
            {
                await connection.OpenAsync(cancellationToken);
                using (var transaction = await connection.BeginTransactionAsync(cancellationToken))
                {
                    var cmdConta = new SqliteCommand("SELECT ativo FROM contacorrente WHERE idcontacorrente = @Id", connection);
                    cmdConta.Parameters.AddWithValue("@Id", request.ContaCorrenteId);
                    var ativo = await cmdConta.ExecuteScalarAsync(cancellationToken) as int?;
                    if (ativo == null) throw new ArgumentException(Messages.ContaInexistente);
                    if (ativo == 0) throw new ArgumentException(Messages.ContaInativa);

                    var cmdMov = new SqliteCommand(@"INSERT INTO movimento (idcontacorrente, valor, tipomovimento) VALUES (@Id, @Valor, @Tipo); SELECT last_insert_rowid();", connection);
                    cmdMov.Parameters.AddWithValue("@Id", request.ContaCorrenteId);
                    cmdMov.Parameters.AddWithValue("@Valor", request.Valor);
                    cmdMov.Parameters.AddWithValue("@Tipo", request.TipoMovimentacao == TipoMovimentacao.Credito ? "C" : "D");

                    var movimentacaoId = (long)await cmdMov.ExecuteScalarAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);

                    return new CriarMovimentacaoResponse { MovimentacaoId = (int)movimentacaoId };
                }
            }
        }
    }
}
