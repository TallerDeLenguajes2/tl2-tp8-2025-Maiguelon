using Microsoft.Data.Sqlite;
using presupuestario;
using tl2_tp8_2025_Maiguelon.Interfaces; // Necesario para usar IUserRepository

// Coloca este archivo en tu namespace de repositorios
public class UsuarioRepository : IUserRepository 
{
    // Cadena de conexión (Asegúrate que la ruta sea correcta)
    private string connectionString = "Data Source=db/tiendadb.db";
    
    public Usuario GetUser(string username, string password)
    {
        Usuario user = null;

        const string sql = @"
        SELECT IdUsuario, Nombre, User, Pass, Rol
        FROM Usuarios
        WHERE User = @Usuario AND Pass = @Contrasena";

        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        
        using var comando = new SqliteCommand(sql, connection);
        // Se usan parámetros para prevenir inyección SQL
        comando.Parameters.AddWithValue("@Usuario", username);
        comando.Parameters.AddWithValue("@Contrasena", password);
        
        using var reader = comando.ExecuteReader();
        
        if (reader.Read()) // Si el lector encuentra una fila, el usuario existe 
        {
            user = new Usuario
            {
                IdUsuario = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                User = reader.GetString(2),
                Pass = reader.GetString(3),
                Rol = reader.GetString(4)
            };
        }
        
        return user;
    }
}