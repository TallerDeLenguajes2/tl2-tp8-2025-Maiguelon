// Coloca este archivo en el namespace donde están Producto y Presupuesto
namespace presupuestario;

public class Usuario
{
    private int idUsuario; 
    private string nombre;
    private string user;
    private string pass;
    private string rol;

    public int IdUsuario { get => idUsuario; set => idUsuario = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string User { get => user; set => user = value; }
    public string Pass { get => pass; set => pass = value; }
    public string Rol { get => rol; set => rol = value; }
}