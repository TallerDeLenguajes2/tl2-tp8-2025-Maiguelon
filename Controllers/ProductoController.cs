using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Maiguelon.Models;
using presupuestario;

namespace tl2_tp8_2025_Maiguelon.Controllers;

public class ProductoController : Controller
{
    private ProductoRepository productoRepository;
    public ProductoController()
    {
        productoRepository = new ProductoRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Producto> productos = productoRepository.Listar();
        return View(productos);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}