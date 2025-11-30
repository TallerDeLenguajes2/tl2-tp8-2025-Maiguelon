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
}