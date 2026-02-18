using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Models;

    public class Presupuesto
    {
        private const decimal IVA = 0.21m; // 21% de IVA
        public int IdPresupuesto { get; set; }
        public string NombreDestinatario { get; set; }
        public DateTime FechaCreacion { get; set; }

        public List<PresupuestoDetalle> Detalle { get; set; } = new List<PresupuestoDetalle>();

        public decimal MontoPresupuesto()
        {
            return Detalle.Sum(d => d.Producto.Precio * d.Cantidad);
        }

        public decimal MontoPresupuestoConIva()
        {
            decimal montoBase = MontoPresupuesto();
            return montoBase * (1 + IVA);
        }
        public int CantidadProductos()
        {
            return Detalle.Sum(d => d.Cantidad);
        }
    }
