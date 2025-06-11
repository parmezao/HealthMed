using HealthMed.Agenda.Application.Interfaces;
using HealthMed.Agenda.Application.ViewModels;
using HealthMed.Agenda.Domain.Entities;
using HealthMed.Agenda.Domain.Interfaces;

namespace HealthMed.Agenda.Application.UseCases
{
    public class AgendaUseCase(IAgendaRepository agendaRepository) : IAgendaUseCase
    {
        public async Task CadastrarHorarioAsync(CadastrarHorarioRequest request)
        {
            var horario = new HorarioDisponivel { MedicoId = request.MedicoId, DataHora = request.DataHora , ValorConsulta = request.ValorConsulta };
            await agendaRepository.AdicionarAsync(horario);
        }

        public async Task<List<HorarioDisponivelResponse>> ObterPorMedicoAsync(int medicoId)
        {
            var horarios = await agendaRepository.ObterPorMedicoAsync(medicoId);
            return horarios.Select(h => new HorarioDisponivelResponse(h.Id, h.MedicoId, h.DataHora, h.Ocupado,h.ValorConsulta)).ToList();
        }

        public async Task<HorarioDisponivelResponse> ObterPorIdAsync(int id)
        {
            var horario = await agendaRepository.ObterPorIdAsync(id);
            if (horario is null)
                throw new ApplicationException("Horário não encontrado!");
            return new HorarioDisponivelResponse(horario.Id, horario.MedicoId, horario.DataHora, horario.Ocupado,horario.ValorConsulta);
        }

        public async Task MarcarComoOcupadoAsync(int horarioId)
        {
            await agendaRepository.MarcarComoOcupadoAsync(horarioId);
        }
    }
}
