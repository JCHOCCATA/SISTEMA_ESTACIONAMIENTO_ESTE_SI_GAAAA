using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;

namespace SistemaEstacionamiento.Services
{
    public class RegistrosEstacionamientoService
    {
        private readonly EstacionamientoModels _context;

        public RegistrosEstacionamientoService(EstacionamientoModels context)
        {
            _context = context;
        }

        public async Task<object> RegistrarIngreso(RegistrosEstacionamiento nuevoRegistro, string usuarioCreacion)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var sitioDb = await _context.Sitios.FindAsync(nuevoRegistro.SitiosFk);
                if (sitioDb == null)
                {
                    return new { success = false, message = "El sitio de estacionamiento seleccionado no existe." };
                }

                if (sitioDb.SitiEstado == false)
                {
                    return new { success = false, message = "Atención: El sitio seleccionado ya se encuentra ocupado en este momento." };
                }

                bool vehiculoAdentro = await _context.RegistrosEstacionamientos
                    .AnyAsync(r => r.VehiculosFk == nuevoRegistro.VehiculosFk && r.ReesEstado == "INGRESADO");

                if (vehiculoAdentro)
                {
                    return new { success = false, message = "El vehículo ya cuenta con un registro de ingreso activo sin salida." };
                }

                nuevoRegistro.ReesFechaIngreso = DateTime.Now;
                nuevoRegistro.ReesEstado = "INGRESADO";
                nuevoRegistro.ReesFechaCreacion = DateTime.Now;
                nuevoRegistro.ReesUsuarioCreacion = usuarioCreacion;
                nuevoRegistro.UsuariosFk = nuevoRegistro.UsuariosFk;

                sitioDb.SitiEstado = false;
                sitioDb.SitiFechaModificacion = DateTime.Now;
                sitioDb.SitiUsuarioModificacion = usuarioCreacion;

                await _context.RegistrosEstacionamientos.AddAsync(nuevoRegistro);
                _context.Sitios.Update(sitioDb);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new { success = true, message = "Ingreso registrado con éxito. El espacio ha sido reservado." };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error en RegistrosEstacionamientoService [Ingreso]: {ex.Message}");
                return new { success = false, message = $"Error interno al procesar el ingreso: {ex.Message}" };
            }
        }

        public async Task<List<object>> ObtenerVehiculosIngresados()
        {
            try
            {
                var lista = await _context.RegistrosEstacionamientos
                    .Include(r => r.VehiculosFkNavigation)
                    .Include(r => r.SitiosFkNavigation)
                    .Where(r => r.ReesEstado == "INGRESADO") // Filtramos solo los que están adentro
                    .OrderByDescending(r => r.ReesFechaIngreso) // El último en ingresar aparece primero
                    .Select(r => new
                    {
                        reesId = r.ReesId,
                        placa = r.VehiculosFkNavigation.VehiPlaca, // Ajusta según tu columna en la tabla Vehiculo
                        sitioNombre = r.SitiosFkNavigation.SitiNombreSitio,
                        fechaIngreso = r.ReesFechaIngreso.ToString("dd/MM/yyyy HH:mm:ss"),
                        estado = r.ReesEstado
                    })
                    .Cast<object>()
                    .ToListAsync();

                return lista;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener vehículos ingresados: {ex.Message}");
                return new List<object>();
            }
        }
    }
}
