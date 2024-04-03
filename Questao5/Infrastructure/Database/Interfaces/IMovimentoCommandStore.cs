using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Database.Interfaces
{
    public interface IMovimentoCommandStore
    {
        /// <summary>
        /// Cria um novo movimento associado a uma conta corrente.
        /// </summary>
        /// <param name="movimento">O movimento a ser persistido no sistema.</param>
        /// <returns>O identificador único do movimento criado.</returns>
        string CreateMovimento(Movimento movimento);

        /// <summary>
        /// Atualiza um movimento existente.
        /// </summary>
        /// <param name="movimento">O movimento com as informações atualizadas.</param>
        void UpdateMovimento(Movimento movimento);

        /// <summary>
        /// Remove um movimento do sistema.
        /// </summary>
        /// <param name="idMovimento">O identificador único do movimento a ser removido.</param>
        void DeleteMovimento(Guid idMovimento);

        /// <summary>
        /// Retorna o valor total dos movimentos de um determinado tipo associados a uma conta corrente.
        /// </summary>
        /// <param name="idContaCorrente"></param>
        /// <param name="tipoMovimento"></param>
        /// <returns></returns>

        decimal SumMovimentosByTipo(string idContaCorrente, TipoMovimentacao tipoMovimento);
    }
}
