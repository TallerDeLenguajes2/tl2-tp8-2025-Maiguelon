using System.ComponentModel.DataAnnotations;
using presupuestario; // Necesario si Presupuesto usa DateOnly de ese namespace (si no, no es necesario)

namespace tl2_tp8_2025_Maiguelon.ViewModels;

public class PresupuestoViewModel
{
    // La ID es necesaria para la acción de Edición (Edit - POST)
    public int IdPresupuesto { get; set; }

    [Display(Name = "Nombre del Destinatario")]
    [Required(ErrorMessage = "El nombre del destinatario es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string NombreDestinatario { get; set; } = string.Empty;

    // La Fecha generalmente no se pide en el formulario de alta/edición,
    // se maneja en el Controller, pero se incluye por completitud.
    public DateOnly FechaCreacion { get; set; }
}