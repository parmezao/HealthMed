using HealthMed.Patient.Application.Events;

namespace HealthMed.Patient.Application.Interfaces
{
    public interface IPacientePublisher
    {
        Task PublishInsertPacienteAsync(InsertPacienteEvent message);
        Task PublishUpdatePacienteAsync(UpdatePacienteEvent message);
        Task PublishDeletePacienteAsync(DeletePacienteEvent message);
}
}
