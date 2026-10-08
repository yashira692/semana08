using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class PrestamosController : Controller
{
    private readonly PrestamoRepositorio _repo;

    public PrestamosController(PrestamoRepositorio repo) => _repo = repo;

    public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta)
    {
        var d = desde ?? DateTime.Today.AddDays(-30);
        var h = hasta ?? DateTime.Today;

        if (d > h)
        {
            TempData["Error"] = "La fecha 'desde' no puede ser mayor que 'hasta'.";
            (d, h) = (h, d);
        }

        ViewData["Desde"] = d.ToString("yyyy-MM-dd");
        ViewData["Hasta"] = h.ToString("yyyy-MM-dd");

        return View(await _repo.ReporteAsync(d, h));
    }
}