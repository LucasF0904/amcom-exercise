using Questao5.Domain.Enumerators;
namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public int IdMovimento { get; set; }
        public string IdContaCorrente { get; set; }
        public string DataMovimento { get; set; }
        public TipoMovimentacao TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}
