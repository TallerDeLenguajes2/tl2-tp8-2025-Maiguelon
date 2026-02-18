namespace MVC.Models;

    public class PresupuestoDetalle
    {
        public int IdPresupuestoDetalle { get; set; } 
        
        public Producto Producto { get; set; }
        
        public int Cantidad { get; set; } 
    }
