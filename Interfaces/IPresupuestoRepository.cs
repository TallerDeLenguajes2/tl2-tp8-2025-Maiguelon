using presupuestario;

namespace tl2_tp8_2025_Maiguelon.Interfaces;

public interface IPresupuestoRepository
{
    List<Presupuesto> ListarPresupuestos();
    // Usamos ObtenerPresupuestoPorId para mantener la consistencia con el CRUD
    Presupuesto ObtenerPresupuestoPorId(int id); 
    int AltaPresupuesto(Presupuesto presupuesto);
    void AgregarDetalle(int idPresupuesto, int idProducto, int cantidad);
    bool ModificarPresupuesto(Presupuesto presupuesto);
    bool EliminarPresupuesto(int id);
}