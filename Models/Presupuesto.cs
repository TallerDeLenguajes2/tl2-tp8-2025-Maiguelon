namespace presupuestario
{
    public class Presupuesto
    {
        private int idPresupuesto;
        private string nombreDestinatario;
        DateOnly fechaCreacion;
        List<PresupuestoDetalle> detalle;

        public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
        public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
        public DateOnly FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
        public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }

        // Metodos

        public int CantidadProductos()
        {
            int numero = 0;
            foreach (var item in detalle!) // El ! evita advertencia de null
            {
                numero += item.Cantidad;
            }
            return numero;
        }

        public double MontoPresupuesto()
        {
            double monto = 0;
            foreach (var item in detalle!)
            {
                if (item.Producto != null) // Solo suma si el Producto NO es nulo
                {
                    monto += item.Cantidad * item.Producto.Precio;
                }
            }
            return monto;
        }

        public double MontoPresupuestoConIva()
        {
            double monto = MontoPresupuesto() * 1.21;
            return monto;
        }
    }
}