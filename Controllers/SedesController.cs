using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEstacionamiento.Models;
using SistemaEstacionamiento.Services;

namespace SistemaEstacionamiento.Controllers
{
    public class SedesController : Controller
    {
        private readonly SedesService _sedesService;

        public SedesController(SedesService sedesService)
        {
            _sedesService = sedesService;
        }
        public ActionResult Sedes()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerSedesJson()
        {
            var data = await _sedesService.ObtenerSedes();
            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarSede(SedeEstacionamiento model)
        {
            string usuarioActivo = User.Identity?.Name ?? "SISTEMA";
            var resultado = await _sedesService.GuardarSede(model, usuarioActivo);
            return Json(new { success = resultado.success, message = resultado.message });
        }

        [HttpPost]
        public async Task<IActionResult> AnularSede(int id)
        {
            string usuarioActivo = User.Identity?.Name ?? "SISTEMA";
            var resultado = await _sedesService.AnularSedeAsync(id, usuarioActivo);
            return Json(new { success = resultado.success, message = resultado.message });
        }
    }
}
