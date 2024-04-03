using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class SaveMovimentoRequest
    {
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public TipoMovimentacao TipoMovimento { get; set; }
    }

}
