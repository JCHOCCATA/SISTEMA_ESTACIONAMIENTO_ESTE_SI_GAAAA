using Microsoft.EntityFrameworkCore;
using SistemaEstacionamiento.Models;

namespace SistemaEstacionamiento.Services
{
    public class UsuariosService
    {
        private readonly EstacionamientoModels _context;
        public UsuariosService(EstacionamientoModels context)
        {
            _context = context;
        }

        public async Task<bool> CrearUsuarioXDefecto()
        {
            try
            {
                bool yaExiste = await _context.Usuarios.AnyAsync(u => u.UsuaNombre == "admin");

                if (yaExiste)
                {
                    Console.WriteLine("El usuario ya existe.");
                    return true;
                }

                string contraseniaPlana = "Admin";
                string contraseniaEncriptada = BCrypt.Net.BCrypt.HashPassword(contraseniaPlana);

                var usuarioDefecto = new Usuario
                {
                    EntidadesFk = 1,
                    RolesFk = 1,
                    SedeEstacionamientosFk = 1,

                    UsuaNombre = "admin",
                    UsuaContrasenia = contraseniaEncriptada,
                    UsuaEstado = true,
                    UsuaFechaCreacion = DateTime.Now,
                    UsuaUsuarioCreacion = "1"
                };

                await _context.Usuarios.AddAsync(usuarioDefecto);
                var filasAfectadas = await _context.SaveChangesAsync();

                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar el usuario por defecto: {ex.Message}");
                return false;
            }
        }
    }
}
