using System.ComponentModel.DataAnnotations;

namespace HealthMed.Patient.Application.ViewModels
{
    public record UpdatePacienteRequest
    {
        public UpdatePacienteRequest(int id, string nome, string cpf, string email)
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
            Email = email;
        }

        [Required(ErrorMessage = "O id é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O id é obrigatório.")]
        public int Id { get; init; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(255, ErrorMessage = "Tamanho inválido, máximo de 255 caracteres.")]
        public string Nome { get; init; }

        [Required(ErrorMessage = "O cpf é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Tamanho inválido, deve ter 11 caracteres.")]
        public string Cpf { get; init; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Este endereço de e-mail não é válido.")]
        [MaxLength(255, ErrorMessage = "Tamanho inválido, máximo de 255 caracteres.")]
        public string Email { get; init; }
    }
}
