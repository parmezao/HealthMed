using HealthMed.Patient.Application.Tests.FakeData.ViewModels;
using HealthMed.Patient.Application.ViewModels;
using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace HealthMed.Patient.Application.Tests.ViewModels
{
    public class InsertPacienteRequestTests
    {
        [Theory]
        [MemberData(nameof(InsertPacienteRequestsFakeData.GetInvalidNomeRequests), MemberType = typeof(InsertPacienteRequestsFakeData))]
        [MemberData(nameof(InsertPacienteRequestsFakeData.GetInvalidCpfRequests), MemberType = typeof(InsertPacienteRequestsFakeData))]
        [MemberData(nameof(InsertPacienteRequestsFakeData.GetInvalidEmailRequests), MemberType = typeof(InsertPacienteRequestsFakeData))]
        public void InsertPacienteRequest_ShouldReturnException_WhenRequestIsInvalid(InsertPacienteRequest request, string expectedErrorMessage)
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(request);

            // Act
            bool isValid = Validator.TryValidateObject(request, context, validationResults, true);

            Assert.False(isValid);
            Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
        }

        [Theory]
        [MemberData(nameof(InsertPacienteRequestsFakeData.GetValidRequests), MemberType = typeof(InsertPacienteRequestsFakeData))]
        public void InsertPacienteRequest_ShouldBeCreatedWithSuccess_WhenIsValid(InsertPacienteRequest request)
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(request);

            // Act
            bool isValid = Validator.TryValidateObject(request, context, validationResults, true);

            Assert.True(isValid);
            Assert.Empty(validationResults);
        }
    }
}
