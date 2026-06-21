using System;
using System.Collections.Generic;

namespace SistemaEstacionamiento.Models;

public partial class Sitio
{
    public int SitiId { get; set; }

    public int SedeEstacionamientosFk { get; set; }

    public string SitiNombreSitio { get; set; } = null!;

    public bool? SitiEstado { get; set; }

    public DateTime? SitiFechaCreacion { get; set; }

    public string? SitiUsuarioCreacion { get; set; }

    public DateTime? SitiFechaModificacion { get; set; }

    public string? SitiUsuarioModificacion { get; set; }

    public virtual ICollection<RegistrosEstacionamiento> RegistrosEstacionamientos { get; set; } = new List<RegistrosEstacionamiento>();

    public virtual SedeEstacionamiento SedeEstacionamientosFkNavigation { get; set; } = null!;
}
