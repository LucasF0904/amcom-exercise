using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Interfaces
{
    public interface IIdempotenciaQueryStore
    {
        /// <summary>
        /// Verifica se uma requisição com a chave de idempotência especificada já foi processada.
        /// </summary>
        /// <param name="chaveIdempotencia">A chave de idempotência da requisição.</param>
        /// <returns>O registro de idempotência, se encontrado; caso contrário, null.</returns>
        Idempotencia FindById(Guid chaveIdempotencia);

        /// <summary>
        /// Salva um novo registro de idempotência no banco de dados.
        /// </summary>
        /// <param name="idempotencia">O objeto de idempotência a ser salvo.</param>
        void Save(Idempotencia idempotencia);
    }
}
