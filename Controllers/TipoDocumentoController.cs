using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEstacionamiento.Services;

namespace SistemaEstacionamiento.Controllers
{
    public class TipoDocumentoController : Controller
    {
        private readonly TipoDocumentoService _tipoDocumentoService;

        public TipoDocumentoController(TipoDocumentoService tipoDocumentoService)
        {
            _tipoDocumentoService = tipoDocumentoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTiposDocumentoJson()
        {
            var resultado = await _tipoDocumentoService.ObtenerTipoDocumentos();
            return Json(resultado);
        }
    }
}
