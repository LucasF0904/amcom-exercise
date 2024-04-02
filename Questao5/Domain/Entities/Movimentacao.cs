using Questao5.Domain.Enumerators;
namespace Questao5.Domain.Entities
{
    public class Movimentacao
    {
        public int Id { get; set; }
        public int ContaCorrenteId { get; set; }
        public decimal Valor { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }
    }
}
