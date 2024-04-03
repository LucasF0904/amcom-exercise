using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Interfaces
{
    public interface IContaCorrenteQueryStore
    {
        /// <summary>
        /// Busca uma conta corrente pelo seu identificador único.
        /// </summary>
        /// <param name="idContaCorrente">O identificador único da conta corrente.</param>
        /// <returns>A conta corrente, se encontrada; caso contrário, null.</returns>
        ContaCorrente FindById(string idContaCorrente);

        /// <summary>
        /// Verifica se uma conta corrente com o número especificado existe e está ativa.
        /// </summary>
        /// <param name="numeroConta">O número da conta corrente.</param>
        /// <returns>True se a conta existir e estiver ativa; caso contrário, false.</returns>
        bool IsContaAtivo(string numeroConta);

        /// <summary>
        /// Obtém o saldo atual de uma conta corrente pelo seu identificador.
        /// </summary>
        /// <remarks>
        /// Este método assume que o saldo pode ser calculado ou recuperado diretamente,
        /// dependendo da implementação específica do armazenamento de dados.
        /// </remarks>
        /// <param name="idContaCorrente">O identificador único da conta corrente.</param>
        /// <returns>O saldo atual da conta.</returns>
        decimal GetSaldoAtual(string idContaCorrente);
    }
}
