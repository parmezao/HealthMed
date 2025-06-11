namespace HealthMed.Agenda.Application.Events
{
    public class CadastrarHorarioEvent
    {
        public int MedicoId { get; set; }
        public DateTime DataHora { get; set; }
        public decimal   ValorConsulta { get; set; }
    }

}
