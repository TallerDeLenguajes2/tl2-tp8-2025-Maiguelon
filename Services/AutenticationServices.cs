using tl2_tp8_2025_Maiguelon.Interfaces; 
using presupuestario; 
using Microsoft.AspNetCore.Http; 

namespace tl2_tp8_2025_Maiguelon.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Inyección de dependencias en el constructor [cite: 797-802]
    public AuthenticationService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public bool Login(string username, string password)
    {
        // 1. Verificar credenciales usando el repositorio
        Usuario usuario = _userRepository.GetUser(username, password);

        if (usuario == null)
        {
            return false; // Login fallido
        }

        // 2. Si existe, crear la sesión [cite: 734, 817-820]
        ISession session = _httpContextAccessor.HttpContext.Session;

        // Guardamos datos clave en la cookie de sesión del servidor
        session.SetString("Usuario", usuario.User);
        session.SetString("Nombre", usuario.Nombre);
        session.SetString("Rol", usuario.Rol);
        session.SetInt32("IdUsuario", usuario.IdUsuario);
        session.SetString("IsAuthenticated", "true");

        return true; // Login exitoso
    }

    public void Logout()
    {
        // Limpiar la sesión actual
        _httpContextAccessor.HttpContext.Session.Clear();
    }
}