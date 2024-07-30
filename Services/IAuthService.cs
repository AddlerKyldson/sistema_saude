using sistema_saude.Models; // Adicione esta linha

namespace sistema_saude.Services
{
    public interface IAuthService
    {
        string Authenticate(LoginModel login);
    }
}
