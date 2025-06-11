using Bogus;
using HealthMed.Patient.Application.ViewModels;

namespace HealthMed.Patient.Application.Tests.FakeData.ViewModels
{
    public static class InsertPacienteRequestsFakeData
    {
        public static IEnumerable<object[]> GetInvalidNomeRequests()
        {
            var faker = new Faker();
            var validEmail = faker.Internet.Email();
            var validCpf = faker.Random.String2(11, "0123456789");

            yield return new object[] { new InsertPacienteRequest("", validCpf, validEmail), "O nome é obrigatório." };
            yield return new object[] { new InsertPacienteRequest(null, validCpf, validEmail), "O nome é obrigatório." };
            yield return new object[] { new InsertPacienteRequest(faker.Random.String(256), validCpf, validEmail), "Tamanho inválido, máximo de 255 caracteres." };
        }

        public static IEnumerable<object[]> GetInvalidCpfRequests()
        {
            var faker = new Faker();
            var validName = faker.Name.FullName();
            var validEmail = faker.Internet.Email();

            yield return new object[] { new InsertPacienteRequest(validName, "", validEmail), "O cpf é obrigatório." };
            yield return new object[] { new InsertPacienteRequest(validName, null, validEmail), "O cpf é obrigatório." };
            yield return new object[] { new InsertPacienteRequest(validName, faker.Random.String2(10, "0123456789"), validEmail), "Tamanho inválido, deve ter 11 caracteres." };
            yield return new object[] { new InsertPacienteRequest(validName, faker.Random.String2(12, "0123456789"), validEmail), "Tamanho inválido, deve ter 11 caracteres." };
        }

        public static IEnumerable<object[]> GetInvalidEmailRequests()
        {
            var faker = new Faker();
            var validName = faker.Name.FullName();
            var validCpf = faker.Random.String2(11, "0123456789");

            yield return new object[] { new InsertPacienteRequest(validName, validCpf, ""), "O e-mail é obrigatório." };
            yield return new object[] { new InsertPacienteRequest(validName, validCpf, null), "O e-mail é obrigatório." };
            yield return new object[] { new InsertPacienteRequest(validName, validCpf, "email"), "Este endereço de e-mail não é válido." };
            yield return new object[] { new InsertPacienteRequest(validName, validCpf, "email@"), "Este endereço de e-mail não é válido." };
            yield return new object[] { new InsertPacienteRequest(validName, validCpf, "email.com"), "Este endereço de e-mail não é válido." };
            //yield return new object[] { new InsertPacienteRequest(validName, validCpf, "email@email."), "Este endereço de e-mail não é válido." };
            yield return new object[] { new InsertPacienteRequest(validName, validCpf, faker.Random.String(256)), "Tamanho inválido, máximo de 255 caracteres." };
        }

        public static IEnumerable<object[]> GetValidRequests()
        {
            var faker = new Faker();

            yield return new object[] { new InsertPacienteRequest(faker.Name.FullName(), faker.Random.String2(11, "0123456789"), faker.Internet.Email()) };
        }
    }
}
