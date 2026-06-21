using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;

namespace SistemaEstacionamiento.Services
{
    public class SitiosService
    {
        private readonly EstacionamientoModels _context;

        public SitiosService(EstacionamientoModels context)
        {
            _context = context;
        }

        public async Task<List<object>> ObtenerListaSitios()
        {
            try
            {
                var lista = await _context.Sitios
                    .Include(s => s.SedeEstacionamientosFkNavigation)
                    .Select(s => new
                    {
                        sitiId = s.SitiId,
                        sitiNombreSitio = s.SitiNombreSitio,
                        sedeNombre = s.SedeEstacionamientosFkNavigation != null
                            ? s.SedeEstacionamientosFkNavigation.SeestNombreEstacionamiento
                            : "Sin Sede",
                        sedeEstacionamientosFk = s.SedeEstacionamientosFk,
                        sitiEstado = s.SitiEstado
                    })
                    .Cast<object>()
                    .ToListAsync();

                return lista;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SitiosService [ObtenerLista]: {ex.Message}");
                return new List<object>();
            }
        }

        public async Task<object> RegistrarSitio(Sitio nuevoSitio, string usuarioCreacion)
        {
            try
            {
                bool sitioDuplicado = await _context.Sitios
                    .AnyAsync(s => s.SitiNombreSitio == nuevoSitio.SitiNombreSitio &&
                                   s.SedeEstacionamientosFk == nuevoSitio.SedeEstacionamientosFk &&
                                   s.SitiEstado == true);

                if (sitioDuplicado)
                {
                    return new { success = false, message = $"El sitio '{nuevoSitio.SitiNombreSitio}' ya se encuentra registrado en esta sede." };
                }

                nuevoSitio.SitiEstado = true;
                nuevoSitio.SitiFechaCreacion = DateTime.Now;
                nuevoSitio.SitiUsuarioCreacion = usuarioCreacion;

                await _context.Sitios.AddAsync(nuevoSitio);
                var filasAfectadas = await _context.SaveChangesAsync();

                if (filasAfectadas > 0)
                {
                    return new { success = true, message = "El sitio de estacionamiento fue registrado con éxito." };
                }

                return new { success = false, message = "No se pudo guardar el registro en la base de datos." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SitiosService [Registrar]: {ex.Message}");
                return new { success = false, message = $"Ocurrió un error interno: {ex.Message}" };
            }
        }

        public async Task<object> EditarSitio(Sitio sitioEditado, string usuarioModificacion)
        {
            try
            {
                var sitioDb = await _context.Sitios.FindAsync(sitioEditado.SitiId);

                if (sitioDb == null)
                {
                    return new { success = false, message = "El sitio que intenta modificar no existe." };
                }

                bool sitioDuplicado = await _context.Sitios
                    .AnyAsync(s => s.SitiNombreSitio == sitioEditado.SitiNombreSitio &&
                                   s.SedeEstacionamientosFk == sitioEditado.SedeEstacionamientosFk &&
                                   s.SitiId != sitioEditado.SitiId &&
                                   s.SitiEstado == true);

                if (sitioDuplicado)
                {
                    return new { success = false, message = $"Ya existe otro sitio llamado '{sitioEditado.SitiNombreSitio}' en esta sede." };
                }

                sitioDb.SedeEstacionamientosFk = sitioEditado.SedeEstacionamientosFk;
                sitioDb.SitiNombreSitio = sitioEditado.SitiNombreSitio;

                sitioDb.SitiFechaModificacion = DateTime.Now;
                sitioDb.SitiUsuarioModificacion = usuarioModificacion;

                _context.Sitios.Update(sitioDb);
                var filasAfectadas = await _context.SaveChangesAsync();

                if (filasAfectadas > 0)
                {
                    return new { success = true, message = "El sitio fue actualizado con éxito." };
                }

                return new { success = false, message = "No se detectaron cambios en el registro." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SitiosService [Editar]: {ex.Message}");
                return new { success = false, message = $"Ocurrió un error interno: {ex.Message}" };
            }
        }

        public async Task<object> AnularSitio(int idSitio, string usuarioModificacion)
        {
            try
            {
                var sitioDb = await _context.Sitios.FindAsync(idSitio);

                if (sitioDb == null)
                {
                    return new { success = false, message = "El sitio seleccionado no existe." };
                }

                sitioDb.SitiEstado = false;
                sitioDb.SitiFechaModificacion = DateTime.Now;
                sitioDb.SitiUsuarioModificacion = usuarioModificacion;

                _context.Sitios.Update(sitioDb);
                var filasAfectadas = await _context.SaveChangesAsync();

                if (filasAfectadas > 0)
                {
                    return new { success = true, message = "El sitio ha sido anulado correctamente." };
                }

                return new { success = false, message = "No se pudo cambiar el estado del sitio." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SitiosService [Anular]: {ex.Message}");
                return new { success = false, message = $"Ocurrió un error interno: {ex.Message}" };
            }
        }

        public async Task<object> RegistrarVariosSitios(int sedeId, int cantidad, string usuarioCreacion)
        {
            try
            {
                var sede = await _context.SedeEstacionamientos.FindAsync(sedeId);
                if (sede == null)
                {
                    return new { success = false, message = "La sede seleccionada no existe." };
                }

                string nombreSede = sede.SeestNombreEstacionamiento;
                List<Sitio> nuevosSitios = new List<Sitio>();

                for (int i = 1; i <= cantidad; i++)
                {
                    var nuevoSitio = new Sitio
                    {
                        SedeEstacionamientosFk = sedeId,
                        SitiNombreSitio = $"Sitio {i} ({nombreSede})",
                        SitiEstado = true,
                        SitiFechaCreacion = DateTime.Now,
                        SitiUsuarioCreacion = usuarioCreacion
                    };
                    nuevosSitios.Add(nuevoSitio);
                }

                await _context.Sitios.AddRangeAsync(nuevosSitios);
                var filasAfectadas = await _context.SaveChangesAsync();

                if (filasAfectadas > 0)
                {
                    return new { success = true, message = $"Se han generado de forma automática {cantidad} sitios correctamente." };
                }

                return new { success = false, message = "No se pudieron guardar los registros." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SitiosService [RegistrarMasivo]: {ex.Message}");
                return new { success = false, message = $"Ocurrió un error interno: {ex.Message}" };
            }
        }

        public async Task<List<object>> ObtenerSitiosPorSedeAsync(int sedeId)
        {
            try
            {
                var sitios = await _context.Sitios
                    .Where(s => s.SedeEstacionamientosFk == sedeId)
                    .Select(s => new
                    {
                        sitiId = s.SitiId,
                        sitiNombreSitio = s.SitiNombreSitio,
                        estaLibre = s.SitiEstado
                    })
                    .Cast<object>()
                    .ToListAsync();

                return sitios;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SitiosService [ObtenerSitiosPorSede]: {ex.Message}");
                return new List<object>();
            }
        }
    }
}
