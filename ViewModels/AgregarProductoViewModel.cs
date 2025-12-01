using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // <-- REQUERIDO

namespace tl2_tp8_2025_Maiguelon.ViewModels;

public class AgregarProductoViewModel
{
    // ID del Presupuesto al que se le está agregando el detalle (hidden field en la vista)
    public int IdPresupuesto { get; set; }

    // ID del Producto seleccionado en el Dropdown
    [Display(Name = "Producto")]
    [Required(ErrorMessage = "Debe seleccionar un producto.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido.")] // Asegura que no es 0
    public int IdProductoSeleccionado { get; set; }

    // Cantidad a agregar
    [Display(Name = "Cantidad")]
    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, 1000, ErrorMessage = "La cantidad debe ser entre 1 y 1000.")]
    public int Cantidad { get; set; }

    // Lista de productos para llenar el Dropdown en la vista (SELECT HTML)
    public SelectList ListaProductos { get; set; }
}