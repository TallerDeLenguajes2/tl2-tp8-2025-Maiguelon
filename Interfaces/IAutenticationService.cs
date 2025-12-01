namespace tl2_tp8_2025_Maiguelon.Interfaces;

public interface IAuthenticationService
{
    bool Login(string username, string password);
    void Logout();
    // bool IsAuthenticated(); // Opcional, útil para verificar estado rápidamente
}