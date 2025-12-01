using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Maiguelon.Models;
using presupuestario;
using tl2_tp8_2025_Maiguelon.ViewModels;

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


    // -----------------------------------------------------------------
    // C R E A T E (ALTA) - Refactorizado con ViewModel
    // -----------------------------------------------------------------

    // GET: Producto/Create (Muestra el formulario)
    [HttpGet]
    public IActionResult Create()
    {
        // Envía un ViewModel vacío al formulario.
        return View(new ProductoViewModel()); 
    }

    // POST: Producto/Create (Recibe el ViewModel)
    [HttpPost]
    public IActionResult Create(ProductoViewModel productoVM)
    {
        // 1. CHEQUEO DE VALIDACIÓN: Si el modelo es inválido (ej. precio negativo), 
        // regresa a la vista para mostrar los errores definidos en el VM.
        if (!ModelState.IsValid) 
        {
            return View(productoVM);
        }

        // 2. MAPPING (VM -> Model de Dominio)
        // Convertimos el objeto seguro y validado (VM) a nuestro objeto de persistencia (Model).
        Producto nuevoProducto = new Producto
        {
            Descripcion = productoVM.Descripcion,
            // (Ajusta el casteo si el tipo de Precio en Producto.cs es diferente a decimal)
            Precio = (int)productoVM.Precio 
        };

        // 3. Persistencia
        productoRepository.Alta(nuevoProducto); 

        return RedirectToAction(nameof(Index));
    }

   // -----------------------------------------------------------------
    // E D I T (EDICIÓN) - Refactorizado con ViewModel
    // -----------------------------------------------------------------

    // GET: Producto/Edit/5 (Carga el formulario)
    [HttpGet]
    public IActionResult Edit(int id)
    {
        Producto productoAEditar = productoRepository.obtenerProducto(id); 

        if (productoAEditar == null) return NotFound(); 

        // 1. MAPPING (Model de Dominio -> VM) para llenar el formulario
        ProductoViewModel productoVM = new ProductoViewModel
        {
            IdProducto = productoAEditar.IdProducto,
            Descripcion = productoAEditar.Descripcion,
            Precio = productoAEditar.Precio
        };

        return View(productoVM);
    }

    // POST: Producto/Edit (Recibe el ViewModel modificado)
    [HttpPost]
    public IActionResult Edit(ProductoViewModel productoVM)
    {
        // 1. CHEQUEO DE VALIDACIÓN
        if (!ModelState.IsValid) 
        {
            return View(productoVM);
        }

        // 2. MAPPING (VM -> Model de Dominio)
        Producto productoModificado = new Producto
        {
            IdProducto = productoVM.IdProducto, 
            Descripcion = productoVM.Descripcion,
            Precio = (int)productoVM.Precio
        };

        // 3. Persistencia
        productoRepository.Modificar(productoModificado.IdProducto, productoModificado);

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