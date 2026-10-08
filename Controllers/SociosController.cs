using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class SociosController : Controller
{
    private readonly SocioRepositorio _repo;

    public SociosController(SocioRepositorio repo) => _repo = repo;

    public async Task<IActionResult> Index() => View(await _repo.ListarAsync());

    [HttpGet]
    public IActionResult Create() => View(new Socio());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Socio socio)
    {
        if (ModelState.IsValid && await _repo.ExisteDniAsync(socio.Dni))
            ModelState.AddModelError(nameof(Socio.Dni), "Ya existe un socio con ese DNI.");

        if (!ModelState.IsValid) return View(socio);

        await _repo.InsertarAsync(socio);
        TempData["Mensaje"] = "Socio registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}