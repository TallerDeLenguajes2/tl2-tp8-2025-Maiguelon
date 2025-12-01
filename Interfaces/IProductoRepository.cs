using presupuestario;

namespace tl2_tp8_2025_Maiguelon.Interfaces;

public interface IProductoRepository
{
    List<Producto> Listar();
    // Usamos el nombre de método que confirmaste:
    Producto obtenerProducto(int id); 
    int Alta(Producto producto);
    bool Modificar(int id, Producto producto);
    bool EliminarProducto(int id);
}