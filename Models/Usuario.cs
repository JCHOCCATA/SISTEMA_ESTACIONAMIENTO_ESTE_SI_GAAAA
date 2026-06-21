using System;
using System.Collections.Generic;

namespace SistemaEstacionamiento.Models;

public partial class Usuario
{
    public int UsuaId { get; set; }

    public int EntidadesFk { get; set; }

    public int RolesFk { get; set; }

    public string UsuaNombre { get; set; } = null!;

    public string UsuaContrasenia { get; set; } = null!;

    public bool? UsuaEstado { get; set; }

    public DateTime? UsuaFechaCreacion { get; set; }

    public string? UsuaUsuarioCreacion { get; set; }

    public DateTime? UsuaFechaModificacion { get; set; }

    public string? UsuaUsuarioModificacion { get; set; }

    public int SedeEstacionamientosFk { get; set; }

    public virtual Entidade EntidadesFkNavigation { get; set; } = null!;

    public virtual ICollection<RegistrosEstacionamiento> RegistrosEstacionamientos { get; set; } = new List<RegistrosEstacionamiento>();

    public virtual Role RolesFkNavigation { get; set; } = null!;

    public virtual SedeEstacionamiento SedeEstacionamientosFkNavigation { get; set; } = null!;
}
