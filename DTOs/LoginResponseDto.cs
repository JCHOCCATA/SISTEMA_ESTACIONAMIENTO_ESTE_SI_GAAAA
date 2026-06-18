namespace SistemaEstacionamiento.DTOs
{
    public class LoginResponseDto
    {
        public int UsuarioId { get; set; }
        public string Username { get; set; } = null!;
        public int SedeId { get; set; }
        public string SedeNombre { get; set; } = null!;
    }
}
