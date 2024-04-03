using Questao5.Domain.Language;

namespace Questao5.Application.Queries.Responses
{
    public class GetSaldoContaCorrenteResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal Saldo { get; set; }
        public string NumeroContaCorrente { get; set; }
        public string NomeTitular { get; set; }
        public DateTime DataHoraConsulta { get; set; }
        public Messages ErrorCode { get; set; }
    }
}
