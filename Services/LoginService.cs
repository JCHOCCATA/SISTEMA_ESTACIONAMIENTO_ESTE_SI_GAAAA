using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.DTOs;
using SistemaEstacionamiento.Models;

namespace SistemaEstacionamiento.Services
{
    public class LoginService
    {
        private readonly EstacionamientoModels _context;

        public LoginService(EstacionamientoModels context)
        {
            _context = context;
        }

        public async Task<LoginResponseDto?> IniciarSesion(string username, string contraseniaPlana)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.SedeEstacionamientosFkNavigation)
                    .FirstOrDefaultAsync(u => u.UsuaNombre == username);

                if (usuario == null || usuario.UsuaEstado == false)
                {
                    return null;
                }

                bool contraseniaValida = BCrypt.Net.BCrypt.Verify(contraseniaPlana, usuario.UsuaContrasenia);

                if (!contraseniaValida)
                {
                    return null;
                }

                var respuesta = new LoginResponseDto
                {
                    UsuarioId = usuario.UsuaId,
                    Username = usuario.UsuaNombre,
                    SedeId = usuario.SedeEstacionamientosFkNavigation.SeestId,
                    SedeNombre = usuario.SedeEstacionamientosFkNavigation.SeestNombreEstacionamiento
                };

                return respuesta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
