using System;
using System.Collections.Generic;

namespace SistemaEstacionamiento.Models;

public partial class TipoVehiculo
{
    public int TiveId { get; set; }

    public string TiveNombreVehiculo { get; set; } = null!;

    public virtual ICollection<PreciosEstacionamiento> PreciosEstacionamientos { get; set; } = new List<PreciosEstacionamiento>();

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
