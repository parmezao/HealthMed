
using HealthMed.Agenda.Domain.Core;
using HealthMed.Agenda.Domain.Entities;
using HealthMed.Agenda.Domain.Interfaces;
using HealthMed.Agenda.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Agenda.Infra.Data.Repositories
{
    public class AgendaRepository(DataContext dataContext) : IAgendaRepository
    {
        public IUnitOfWork UnitOfWork => dataContext;

        public async Task AdicionarAsync(HorarioDisponivel horario)
        {
            dataContext.HorariosDisponiveis.Add(horario);
        }

        public async Task AtualizarAsync(HorarioDisponivel horario)
        {
            dataContext.HorariosDisponiveis.Update(horario);
        }

        public void Dispose()
        {
            dataContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task MarcarComoOcupadoAsync(int id)
        {
            var horario = await dataContext.HorariosDisponiveis.FirstOrDefaultAsync(x => x.Id == id);
            if (horario is not null)
            {
                horario.Ocupado = true;
                dataContext.HorariosDisponiveis.Update(horario);
            }
        }

        public async Task<HorarioDisponivel?> ObterPorIdAsync(int id)
        {
            return await dataContext.HorariosDisponiveis.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<HorarioDisponivel>> ObterPorMedicoAsync(int medicoId)
        {
            return await dataContext.HorariosDisponiveis
            .Where(h => h.MedicoId == medicoId && !h.Ocupado)
            .OrderBy(h => h.DataHora)
            .ToListAsync();
        }

        public async Task RemoverAsync(HorarioDisponivel horario)
        {
            dataContext.HorariosDisponiveis.Remove(horario);
        }
    }
}
