using System.ComponentModel.DataAnnotations;
using MVC.Models;

namespace MVC.ViewModels;

public class ProductoViewModel
{
    public ProductoViewModel()
    { 

    }

    public ProductoViewModel(Producto producto)
    {
        Descripcion = producto.Descripcion;
        IdProducto = producto.IdProducto;
        Precio = producto.Precio;
    }
    public int IdProducto { get; set; } 
    // ❗ Validación: Máximo 250 caracteres. Es opcional si no tiene [Required]
    [Display(Name = "Titulo de la Pelicula")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El Titulo debe tener entre 2 y 100 caracteres.")]
    public string Descripcion { get; set; }

    // USARIA esto para poner un limite al anio, pero no quiero perder tiempo obteniendo el final del string
    // DateOnly hoy = DateOnly.FromDateTime(DateTime.Now);
    // int hoyInt = int.Parse(hoy);

    // ❗ Validación: Requerido y debe ser positivo
    [Display(Name = "Precio Unitario")]
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(1800, 2025, ErrorMessage = "El Anio debe tener sentido.")] 
    public decimal Precio { get; set; }
}
