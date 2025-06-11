namespace HealthMed.Agenda.Application.Events
{
    public class EditarHorarioEvent
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
    }
}
