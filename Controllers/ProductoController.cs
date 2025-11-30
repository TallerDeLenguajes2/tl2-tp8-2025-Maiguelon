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


    // GET: Producto/Create (Muestra el formulario vacío)
    public IActionResult Create()
    {
        // Simplemente devuelve una vista sin modelo, porque el formulario estará vacío.
        return View();
    }

    // POST: Producto/Create (Recibe el formulario y guarda)
    [HttpPost]
    public IActionResult Create(Producto producto)
    {
        // Llama al repositorio para guardar el nuevo producto
        productoRepository.Alta(producto);

        // Redirecciona al listado para mostrar el resultado (Patrón Post-Redirect-Get)
        return RedirectToAction(nameof(Index));
    }

    // GET: Producto/Edit/5 (Muestra el formulario con los datos cargados)
    [HttpGet]
    public IActionResult Edit(int id)
    {
        // 1. Buscar el producto por ID en el Repositorio
        Producto productoAEditar = productoRepository.obtenerProducto(id);

        if (productoAEditar == null)
        {
            return NotFound();
        }

        // 2. Si existe, enviar el objeto a la vista Edit.cshtml
        return View(productoAEditar);
    }

    // POST: Producto/Edit (Recibe el formulario modificado y guarda)
    [HttpPost]
    public IActionResult Edit(Producto producto)
    {
        // 1. Llamar al repositorio para ejecutar el UPDATE
        productoRepository.Modificar(producto.IdProducto, producto);

        // 2. Redirigir al listado
        return RedirectToAction(nameof(Index));
    }

    // 1. GET: Mostrar la página de confirmación
    [HttpGet]
    public IActionResult Delete(int id)
    {
        Producto productoAEliminar = productoRepository.obtenerProducto(id);

        if (productoAEliminar == null)
        {
            return NotFound();
        }

        return View(productoAEliminar);
    }

    // 2. POST: Ejecutar la eliminación (Nombre único + atributo ActionName)
    [HttpPost]
    [ActionName("Delete")]
    // Es necesario cambiar el name solo aquí porque es el único método que recibe los mismos parámetros
    public IActionResult DeleteConfirmed(int id) 
    {
        bool eliminado = productoRepository.eliminarProducto(id);

        if (!eliminado)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}