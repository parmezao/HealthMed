namespace HealthMed.Agenda.Application.ViewModels
{
    public record HorarioDisponivelResponse(int Id, int MedicoId, DateTime DataHora, bool Ocupado, decimal ValorConsulta);
}
