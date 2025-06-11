using HealthMed.Patient.Application.Tests.FakeData.ViewModels;
using HealthMed.Patient.Application.ViewModels;
using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace HealthMed.Patient.Application.Tests.ViewModels
{
    public class UpdatePacienteRequestTests
    {
        [Theory]
        [MemberData(nameof(UpdatePacienteRequestsFakeData.GetInvalidIdRequests), MemberType = typeof(UpdatePacienteRequestsFakeData))]
        [MemberData(nameof(UpdatePacienteRequestsFakeData.GetInvalidNomeRequests), MemberType = typeof(UpdatePacienteRequestsFakeData))]
        [MemberData(nameof(UpdatePacienteRequestsFakeData.GetInvalidCpfRequests), MemberType = typeof(UpdatePacienteRequestsFakeData))]
        [MemberData(nameof(UpdatePacienteRequestsFakeData.GetInvalidEmailRequests), MemberType = typeof(UpdatePacienteRequestsFakeData))]
        public void UpdatePacienteRequest_ShouldReturnException_WhenRequestIsInvalid(UpdatePacienteRequest request, string expectedErrorMessage)
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
        [MemberData(nameof(UpdatePacienteRequestsFakeData.GetValidRequests), MemberType = typeof(UpdatePacienteRequestsFakeData))]
        public void InsertPacienteRequest_ShouldBeCreatedWithSuccess_WhenIsValid(UpdatePacienteRequest request)
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
