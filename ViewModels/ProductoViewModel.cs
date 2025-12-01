using System.ComponentModel.DataAnnotations;

namespace tl2_tp8_2025_Maiguelon.ViewModels;

public class ProductoViewModel
{
    // La ID es necesaria para la acción de Edición (Edit - POST)
    public int IdProducto { get; set; }

    [Display(Name = "Descripción")] // Nombre que se mostrará en las etiquetas (opcional, pero útil)
    [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
    public string Descripcion { get; set; } 
    // Nota: Usar 'string?' permite que sea un campo opcional, lo cual está alineado con solo usar StringLength.
    
    [Display(Name = "Precio Unitario")]
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
    public decimal Precio { get; set; }
}