using System;
using System.Collections.Generic;

namespace SistemaEstacionamiento.Models;

public partial class SedeEstacionamiento
{
    public int SeestId { get; set; }

    public string SeestNombreEstacionamiento { get; set; } = null!;

    public string SeestUbicacion { get; set; } = null!;

    public bool? SeestEstado { get; set; }

    public DateTime? SeestFechaCreacion { get; set; }

    public string? SeestUsuarioCreacion { get; set; }

    public DateTime? SeestFechaModificacion { get; set; }

    public string? SeestUsuarioModificacion { get; set; }

    public virtual ICollection<PreciosEstacionamiento> PreciosEstacionamientos { get; set; } = new List<PreciosEstacionamiento>();

    public virtual ICollection<Sitio> Sitios { get; set; } = new List<Sitio>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
