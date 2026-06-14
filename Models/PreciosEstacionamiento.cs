using System;
using System.Collections.Generic;

namespace SistemaEstacionamiento.Models;

public partial class PreciosEstacionamiento
{
    public int PreeId { get; set; }

    public int TipoVehiculoFk { get; set; }

    public int SedeEstacionamientosFk { get; set; }

    public decimal PreePrecio { get; set; }

    public bool? PreeEstado { get; set; }

    public DateTime? PreeFechaCreacion { get; set; }

    public string? PreeUsuarioCreacion { get; set; }

    public DateTime? PreeFechaModificacion { get; set; }

    public string? PreeUsuarioModificacion { get; set; }

    public virtual SedeEstacionamiento SedeEstacionamientosFkNavigation { get; set; } = null!;

    public virtual TipoVehiculo TipoVehiculoFkNavigation { get; set; } = null!;
}
