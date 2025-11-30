using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Maiguelon.Models;
using presupuestario;

namespace tl2_tp8_2025_Maiguelon.Controllers;

public class PresupuestoController : Controller
{
    private PresupuestoRepository presupuestoRepository;
    public PresupuestoController()
    {
        presupuestoRepository = new PresupuestoRepository();
    }

    public IActionResult Index()
    {
        List<Presupuesto> presupuestos = presupuestoRepository.ListarPresupuestos();
        return View(presupuestos);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Detalle(int id)
    {
        Presupuesto presupuesto = presupuestoRepository.ObtenerPresupuestoPorId(id);

        if (presupuesto == null)
        {
            return NotFound();
        }
        return View(presupuesto);
    }

    // --- C R E A T E (ALTA DE ENCABEZADO) ---

    // GET: Presupuesto/Create
    [HttpGet]
    public IActionResult Create()
    {
        // En un caso real, aquí cargarías la lista de productos
        return View();
    }

    // POST: Presupuesto/Create
    [HttpPost]
    public IActionResult Create(Presupuesto presupuesto)
    {
        // Nota: Por simplicidad, este método asume que solo se ingresa el NombreDestinatario
        // y la FechaCreacion, y que el Detalle se agregará en una acción posterior.

        // Establecer la fecha actual si no se establece en el formulario
        if (presupuesto.FechaCreacion == DateOnly.MinValue) 
        {
             presupuesto.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
        }
        
        presupuestoRepository.AltaPresupuesto(presupuesto); 

        // Redirige al listado
        return RedirectToAction(nameof(Index));
    }

    // --- U P D A T E (EDICIÓN DE ENCABEZADO) ---

    // GET: Presupuesto/Edit/5
    [HttpGet]
    public IActionResult Edit(int id)
    {
        Presupuesto presupuestoAEditar = presupuestoRepository.ObtenerPresupuestoPorId(id); 

        if (presupuestoAEditar == null)
        {
            return NotFound(); 
        }
        return View(presupuestoAEditar);
    }

    // POST: Presupuesto/Edit
    [HttpPost]
    public IActionResult Edit(Presupuesto presupuesto)
    {
        // ModificarPresupuesto debe existir en tu repositorio.
        presupuestoRepository.ModificarPresupuesto(presupuesto);

        return RedirectToAction(nameof(Index));
    }

    // --- D E L E T E (ELIMINACIÓN) ---
    
    // GET: Presupuesto/Delete/5 (Muestra la confirmación)
    [HttpGet]
    public IActionResult Delete(int id)
    {
        Presupuesto presupuestoAEliminar = presupuestoRepository.ObtenerPresupuestoPorId(id); 

        if (presupuestoAEliminar == null)
        {
            return NotFound();
        }
        return View(presupuestoAEliminar);
    }

    // POST: Presupuesto/Delete (Ejecuta la eliminación)
    [HttpPost]
    [ActionName("Delete")] // Resuelve la ambigüedad con el GET
    public IActionResult DeleteConfirmed(int id)
    {
        // EliminarPresupuesto debe manejar la eliminación del encabezado y sus detalles
        bool eliminado = presupuestoRepository.EliminarPresupuesto(id);

        if (!eliminado)
        {
            return NotFound(); 
        }
        return RedirectToAction(nameof(Index));
    }
}