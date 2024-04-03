using Questao5.Domain.Language;

namespace Questao5.Application.Commands.Responses
{
    public class CreateMovimentoResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string MovimentoId { get; set; }
        public Messages ErrorCode { get; set; }
    }
}
