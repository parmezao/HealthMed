using Bogus;
using HealthMed.Patient.Application.ViewModels;

namespace HealthMed.Patient.Application.Tests.FakeData.ViewModels
{
    public static class UpdatePacienteRequestsFakeData
    {
        public static IEnumerable<object[]> GetInvalidNomeRequests()
        {
            var faker = new Faker();
            var validId = faker.Random.Int(1, int.MaxValue);
            var validEmail = faker.Internet.Email();
            var validCpf = faker.Random.String2(11, "0123456789");

            yield return new object[] { new UpdatePacienteRequest(validId, "", validCpf, validEmail), "O nome é obrigatório." };
            yield return new object[] { new UpdatePacienteRequest(validId, null, validCpf, validEmail), "O nome é obrigatório." };
            yield return new object[] { new UpdatePacienteRequest(validId, faker.Random.String(256), validCpf, validEmail), "Tamanho inválido, máximo de 255 caracteres." };
        }

        public static IEnumerable<object[]> GetInvalidIdRequests()
        {
            var faker = new Faker();
            var validName = faker.Name.FullName();
            var validCpf = faker.Random.String2(11, "0123456789");
            var validEmail = faker.Internet.Email();

            yield return new object[] { new UpdatePacienteRequest(0, validName, validCpf, validEmail), "O id é obrigatório." };
        }

        public static IEnumerable<object[]> GetInvalidCpfRequests()
        {
            var faker = new Faker();
            var validId = faker.Random.Int(1, int.MaxValue);
            var validName = faker.Name.FullName();
            var validEmail = faker.Internet.Email();

            yield return new object[] { new UpdatePacienteRequest(validId, validName, "", validEmail), "O cpf é obrigatório." };
            yield return new object[] { new UpdatePacienteRequest(validId, validName, null, validEmail), "O cpf é obrigatório." };
            yield return new object[] { new UpdatePacienteRequest(validId, validName, faker.Random.String2(10, "0123456789"), validEmail), "Tamanho inválido, deve ter 11 caracteres." };
            yield return new object[] { new UpdatePacienteRequest(validId, validName, faker.Random.String2(12, "0123456789"), validEmail), "Tamanho inválido, deve ter 11 caracteres." };
        }

        public static IEnumerable<object[]> GetInvalidEmailRequests()
        {
            var faker = new Faker();
            var validId = faker.Random.Int(1, int.MaxValue);
            var validName = faker.Name.FullName();
            var validCpf = faker.Random.String2(11, "0123456789");

            yield return new object[] { new UpdatePacienteRequest(validId, validName, validCpf, ""), "O e-mail é obrigatório." };
            yield return new object[] { new UpdatePacienteRequest(validId, validName, validCpf, null), "O e-mail é obrigatório." };
            yield return new object[] { new UpdatePacienteRequest(validId, validName, validCpf, "email"), "Este endereço de e-mail não é válido." };
            yield return new object[] { new UpdatePacienteRequest(validId, validName, validCpf, "email@"), "Este endereço de e-mail não é válido." };
            yield return new object[] { new UpdatePacienteRequest(validId, validName, validCpf, "email.com"), "Este endereço de e-mail não é válido." };
            //yield return new object[] { new UpdatePacienteRequest(validId, validName, validCpf, "email@email."), "Este endereço de e-mail não é válido." };
            yield return new object[] { new UpdatePacienteRequest(validId, validName, validCpf, faker.Random.String(256)), "Tamanho inválido, máximo de 255 caracteres." };
        }

        public static IEnumerable<object[]> GetValidRequests()
        {
            var faker = new Faker();

            yield return new object[] { new UpdatePacienteRequest(faker.Random.Int(1, int.MaxValue), faker.Name.FullName(), faker.Random.String2(11, "0123456789"), faker.Internet.Email()) };
        }
    }
}
