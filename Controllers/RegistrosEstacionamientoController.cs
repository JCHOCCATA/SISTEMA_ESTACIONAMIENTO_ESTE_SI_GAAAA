using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEstacionamiento.Models;
using SistemaEstacionamiento.Services;
using System.Security.Claims;

namespace SistemaEstacionamiento.Controllers
{
    public class RegistrosEstacionamientoController : Controller
    {
        private readonly RegistrosEstacionamientoService _registrosService;

        public RegistrosEstacionamientoController(RegistrosEstacionamientoService registrosService)
        {
            _registrosService = registrosService;
        }
        [HttpGet]
        public ActionResult RegistrosEstacionamiento()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerIngresadosJson()
        {
            var resultado = await _registrosService.ObtenerVehiculosIngresados();
            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarIngreso(RegistrosEstacionamiento registro)
        {

            string usuarioName = User.Identity?.Name ?? "SISTEMA";
            string usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0";

            if (int.TryParse(usuarioIdClaim, out int idUsuarioLogueado))
            {
                registro.UsuariosFk = idUsuarioLogueado;
            }
            else
            {
                return Json(new { success = false, message = "No se pudo identificar al usuario de la sesión actual." });
            }

            var resultado = await _registrosService.RegistrarIngreso(registro, usuarioName);

            return Json(resultado);
        }
    }
}
