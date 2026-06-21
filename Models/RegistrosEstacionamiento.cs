using System;
using System.Collections.Generic;

namespace SistemaEstacionamiento.Models;

public partial class RegistrosEstacionamiento
{
    public int ReesId { get; set; }

    public int VehiculosFk { get; set; }

    public int SitiosFk { get; set; }

    public int UsuariosFk { get; set; }

    public DateTime ReesFechaIngreso { get; set; }

    public DateTime? ReesFechaSalida { get; set; }

    public int? ReesTiempoMinutos { get; set; }

    public decimal? ReesMontoCobrado { get; set; }

    public string? ReesEstado { get; set; }

    public DateTime? ReesFechaCreacion { get; set; }

    public string? ReesUsuarioCreacion { get; set; }

    public virtual Sitio SitiosFkNavigation { get; set; } = null!;

    public virtual Usuario UsuariosFkNavigation { get; set; } = null!;

    public virtual Vehiculo VehiculosFkNavigation { get; set; } = null!;
}
