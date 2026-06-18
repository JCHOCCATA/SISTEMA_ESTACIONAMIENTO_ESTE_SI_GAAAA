using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEstacionamiento.Models;
using SistemaEstacionamiento.Services;
using System.Security.Claims;

namespace SistemaEstacionamiento.Controllers
{
    public class EntidadesController : Controller
    {
        private readonly EntidadesService _entidadesService;

        public EntidadesController(EntidadesService entidadesService)
        {
            _entidadesService = entidadesService;
        }

        [HttpGet]
        public IActionResult Entidades()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarEntidad(Entidade entidad)
        {
            string usuarioActual = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "SISTEMA";
            object resultado;

            if (entidad.EntiId == 0)
            {
                // Si el ID es 0, es una nueva inserción
                resultado = await _entidadesService.RegistrarEntidad(entidad, usuarioActual);
            }
            else
            {
                // Si trae ID, es una edición
                resultado = await _entidadesService.EditarEntidad(entidad, usuarioActual);
            }

            return Json(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEntidadesJson()
        {
            var resultado = await _entidadesService.ObtenerListaEntidades();
            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> AnularEntidad(int id)
        {
            string usuarioActual = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "SISTEMA";
            var resultado = await _entidadesService.AnularEntidad(id, usuarioActual);
            return Json(resultado);
        }
    }
}
