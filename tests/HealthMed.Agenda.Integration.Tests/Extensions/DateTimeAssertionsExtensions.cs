using FluentAssertions;

namespace HealthMed.Agenda.Integration.Tests.Extensions
{
    public static class DateTimeAssertionsExtensions
    {
        public static void ShouldContainDateCloseTo(this IEnumerable<DateTime> lista, DateTime esperado, TimeSpan tolerancia)
        {
            lista.Any(data =>
                data >= esperado - tolerancia &&
                data <= esperado + tolerancia
            ).Should().BeTrue($"Esperava-se um DateTime dentro de {tolerancia.TotalSeconds} segundos de {esperado}.");
        }
    }
}
