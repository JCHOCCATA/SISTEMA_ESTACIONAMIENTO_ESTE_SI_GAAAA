using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;
using SistemaEstacionamiento.Services;
using System.Security.Claims;

namespace SistemaEstacionamiento.Controllers
{
    public class SitiosController : Controller
    {
        private readonly SitiosService _sitiosService;
        private readonly SedesService _sedesService;

        public SitiosController(SitiosService sitiosService, SedesService sedesService)
        {
            _sitiosService = sitiosService;
            _sedesService = sedesService;
        }

        [HttpGet]
        public IActionResult Sitios()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerSitiosJson()
        {
            var resultado = await _sitiosService.ObtenerListaSitios();
            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarSitio(int SitiId, int SedeEstacionamientosFk, string SitiNombreSitio, int? CantidadSitios)
        {
            string usuarioActual = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "SISTEMA";
            object resultado;

            if (SitiId == 0)
            {
                int cantidad = CantidadSitios ?? 1;
                resultado = await _sitiosService.RegistrarVariosSitios(SedeEstacionamientosFk, cantidad, usuarioActual);
            }
            else
            {
                var sitioEditar = new Sitio
                {
                    SitiId = SitiId,
                    SedeEstacionamientosFk = SedeEstacionamientosFk,
                    SitiNombreSitio = SitiNombreSitio
                };
                resultado = await _sitiosService.EditarSitio(sitioEditar, usuarioActual);
            }

            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> AnularSitio(int id)
        {
            string usuarioActual = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "SISTEMA";
            var resultado = await _sitiosService.AnularSitio(id, usuarioActual);
            return Json(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerSitiosPorSedeJson(int sedeId)
        {
            var resultado = await _sitiosService.ObtenerSitiosPorSedeAsync(sedeId);
            return Json(resultado);
        }

    }
}
