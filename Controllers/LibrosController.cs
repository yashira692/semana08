using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Web.Controllers;

public class LibrosController : Controller
{
    private readonly LibroRepositorio _repo;

    public LibrosController(LibroRepositorio repo) => _repo = repo;

    private async Task CargarAutoresAsync(int? seleccionado = null)
    {
        var autores = await _repo.ListarAutoresAsync();
        ViewData["Autores"] = new SelectList(autores, "AutorId", "Nombre", seleccionado);
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        ViewData["Buscar"] = buscar;
        var libros = string.IsNullOrWhiteSpace(buscar)
            ? await _repo.ListarAsync()
            : await _repo.BuscarAsync(buscar.Trim());
        return View(libros);
    }

    public async Task<IActionResult> Details(int id)
    {
        var libro = await _repo.ObtenerAsync(id);
        if (libro is null) return NotFound();
        return View(libro);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await CargarAutoresAsync();
        return View(new Libro());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }
        await _repo.InsertarAsync(libro);
        TempData["Mensaje"] = "Libro registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var libro = await _repo.ObtenerAsync(id);
        if (libro is null) return NotFound();
        await CargarAutoresAsync(libro.AutorId);
        return View(libro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Libro libro)
    {
        if (id != libro.LibroId) return BadRequest();
        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }
        await _repo.ActualizarAsync(libro);
        TempData["Mensaje"] = "Libro actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var libro = await _repo.ObtenerAsync(id);
        if (libro is null) return NotFound();
        return View(libro);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repo.EliminarAsync(id);
        TempData["Mensaje"] = "Libro eliminado (baja lógica).";
        return RedirectToAction(nameof(Index));
    }
}