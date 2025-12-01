using presupuestario; 

namespace tl2_tp8_2025_Maiguelon.Interfaces;

public interface IUserRepository
{
    // Retorna el objeto Usuario si las credenciales son válidas, sino null.
    Usuario GetUser(string username, string password);
}