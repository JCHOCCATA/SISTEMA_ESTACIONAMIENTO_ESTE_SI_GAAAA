using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;

namespace SistemaEstacionamiento.Services
{
    public class TipoDocumentoService
    {
        private readonly EstacionamientoModels _context;

        public TipoDocumentoService(EstacionamientoModels context)
        {
            _context = context;
        }

        public async Task<List<object>> ObtenerTipoDocumentos()
        {
            try
            {
                var lista = await _context.TipoDocumentos
                    .Select(t => new
                    {
                        tidoId = t.TidoId,
                        tidoNombreDoc = t.TidoNombreDoc
                    })
                    .Cast<object>()
                    .ToListAsync();

                return lista;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en TipoDocumentoService [ObtenerCombo]: {ex.Message}");
                return new List<object>();
            }
        }
    }
}
