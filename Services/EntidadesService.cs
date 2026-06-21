using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaEstacionamiento.Services
{
    public class EntidadesService
    {
        private readonly EstacionamientoModels _context;

        public EntidadesService(EstacionamientoModels context)
        {
            _context = context;
        }

        public async Task<object> RegistrarEntidad(Entidade nuevaEntidad, string usuarioCreacion)
        {
            try
            {
                bool documentoDuplicado = await _context.Entidades
                    .AnyAsync(e => e.EntiNroDocumento == nuevaEntidad.EntiNroDocumento);

                if (documentoDuplicado)
                {
                    return new { success = false, message = $"El número de documento '{nuevaEntidad.EntiNroDocumento}' ya se encuentra registrado." };
                }

                // CAMBIO AQUÍ: Asignar como string ("1" o "ACTIVO") según tu BD
                nuevaEntidad.EntiEstado = "1";
                nuevaEntidad.EntiFechaCreacion = DateTime.Now;
                nuevaEntidad.EntiUsuarioCreacion = usuarioCreacion;

                await _context.Entidades.AddAsync(nuevaEntidad);
                var filasAfectadas = await _context.SaveChangesAsync();

                if (filasAfectadas > 0)
                {
                    return new { success = true, message = "La entidad fue registrada con éxito." };
                }

                return new { success = false, message = "No se pudo completar el registro en la base de datos." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EntidadesService [Registrar]: {ex.Message}");
                return new { success = false, message = $"Ocurrió un error interno: {ex.Message}" };
            }
        }

        public async Task<List<object>> ObtenerListaEntidades()
        {
            try
            {
                var lista = await _context.Entidades
                    .Include(e => e.TipoDocumentoFkNavigation)
                    .Select(e => new
                    {
                        entiId = e.EntiId,
                        entiRazonSocial = e.EntiRazonSocial,
                        tipoDocumento = e.TipoDocumentoFkNavigation.TidoNombreDoc,
                        tipoDocumentoFk = e.TipoDocumentoFk,
                        entiNroDocumento = e.EntiNroDocumento,
                        // CAMBIO AQUÍ: Compara contra el string correspondiente
                        entiEstado = e.EntiEstado == "1"
                    })
                    .Cast<object>()
                    .ToListAsync();

                return lista;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EntidadesService [ObtenerLista]: {ex.Message}");
                return new List<object>();
            }
        }

        public async Task<object> EditarEntidad(Entidade entidadEditada, string usuarioModificacion)
        {
            try
            {
                var entidadDb = await _context.Entidades.FindAsync(entidadEditada.EntiId);

                if (entidadDb == null)
                {
                    return new { success = false, message = "La entidad que intenta modificar no existe." };
                }

                bool documentoDuplicado = await _context.Entidades
                    .AnyAsync(e => e.EntiNroDocumento == entidadEditada.EntiNroDocumento && e.EntiId != entidadEditada.EntiId);

                if (documentoDuplicado)
                {
                    return new { success = false, message = $"El número de documento '{entidadEditada.EntiNroDocumento}' ya está asignado a otra entidad." };
                }

                entidadDb.TipoDocumentoFk = entidadEditada.TipoDocumentoFk;
                entidadDb.EntiRazonSocial = entidadEditada.EntiRazonSocial;
                entidadDb.EntiNroDocumento = entidadEditada.EntiNroDocumento;

                entidadDb.EntiFechaModificacion = DateTime.Now;
                entidadDb.EntiUsuarioModificacion = usuarioModificacion;

                _context.Entidades.Update(entidadDb);
                var filasAfectadas = await _context.SaveChangesAsync();

                if (filasAfectadas > 0)
                {
                    return new { success = true, message = "La entidad fue actualizada con éxito." };
                }

                return new { success = false, message = "No se realizaron cambios en la entidad." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EntidadesService [Editar]: {ex.Message}");
                return new { success = false, message = $"Ocurrió un error interno: {ex.Message}" };
            }
        }

        public async Task<object> AnularEntidad(int idEntidad, string usuarioModificacion)
        {
            try
            {
                var entidadDb = await _context.Entidades.FindAsync(idEntidad);

                if (entidadDb == null)
                {
                    return new { success = false, message = "La entidad no existe." };
                }

                // CAMBIO AQUÍ: Asignar como string ("0" o "ANULADO")
                entidadDb.EntiEstado = "0";
                entidadDb.EntiFechaModificacion = DateTime.Now;
                entidadDb.EntiUsuarioModificacion = usuarioModificacion;

                _context.Entidades.Update(entidadDb);
                var filasAfectadas = await _context.SaveChangesAsync();

                if (filasAfectadas > 0)
                {
                    return new { success = true, message = "La entidad fue anulada correctamente." };
                }

                return new { success = false, message = "No se pudo cambiar el estado de la entidad." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EntidadesService [Anular]: {ex.Message}");
                return new { success = false, message = $"Ocurrió un error interno: {ex.Message}" };
            }
        }
    }
}