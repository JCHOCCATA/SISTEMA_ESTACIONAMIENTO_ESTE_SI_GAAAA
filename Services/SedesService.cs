using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;

namespace SistemaEstacionamiento.Services
{
    public class SedesService
    {
        private readonly EstacionamientoModels _context;

        public SedesService(EstacionamientoModels context)
        {
            _context = context;
        }
        public async Task<List<object>> ObtenerSedes()
        {
            try
            {
                return await _context.SedeEstacionamientos
                    .Where(s => s.SeestEstado == true)
                    .Select(s => new
                    {
                        seestId = s.SeestId,
                        seestNombreEstacionamiento = s.SeestNombreEstacionamiento,
                        seestUbicacion = s.SeestUbicacion,
                        seestEstado = s.SeestEstado
                    })
                    .Cast<object>()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SedesService [ObtenerSedesJsonAsync]: {ex.Message}");
                return new List<object>();
            }
        }

        public async Task<(bool success, string message)> GuardarSede(SedeEstacionamiento model, string usuarioId)
        {
            try
            {
                if (model.SeestId == 0)
                {
                    model.SeestEstado = true;
                    model.SeestFechaCreacion = DateTime.Now;
                    model.SeestUsuarioCreacion = usuarioId;

                    await _context.SedeEstacionamientos.AddAsync(model);
                    await _context.SaveChangesAsync();
                    return (true, "Sede de estacionamiento registrada correctamente.");
                }

                var existe = await _context.SedeEstacionamientos.FindAsync(model.SeestId);
                if (existe == null) return (false, "La sede seleccionada no existe.");

                existe.SeestNombreEstacionamiento = model.SeestNombreEstacionamiento;
                existe.SeestUbicacion = model.SeestUbicacion;
                existe.SeestFechaModificacion = DateTime.Now;
                existe.SeestUsuarioModificacion = usuarioId;

                _context.SedeEstacionamientos.Update(existe);
                await _context.SaveChangesAsync();
                return (true, "Datos de la sede actualizados correctamente.");
            }
            catch (Exception ex)
            {
                return (false, $"Error interno al guardar: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> AnularSedeAsync(int id, string usuarioId)
        {
            try
            {
                var sede = await _context.SedeEstacionamientos.FindAsync(id);
                if (sede == null) return (false, "La sede que intenta anular no existe.");

                sede.SeestEstado = false;
                sede.SeestFechaModificacion = DateTime.Now;
                sede.SeestUsuarioModificacion = usuarioId;

                _context.SedeEstacionamientos.Update(sede);
                await _context.SaveChangesAsync();
                return (true, "Sede anulada del sistema con éxito.");
            }
            catch (Exception ex)
            {
                return (false, $"Fallo al intentar anular: {ex.Message}");
            }
        }

    }
}
