using HealthMed.Auth.Application.ViewModels;

namespace HealthMed.Auth.Application.Interfaces
{
    public interface ILoginUseCase
    {
        Task<LoginResponse> ExecuteAsync(LoginRequest request);
    }
}
