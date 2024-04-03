namespace Questao5.Infrastructure.Database.CommandStore.Responses
{
    public class SaveMovimentoResponse
    {
        public bool Success { get; set; }
        public Guid MovimentoId { get; set; }
    }
}
