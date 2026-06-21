using System;
using System.Collections.Generic;

namespace SistemaEstacionamiento.Models;

public partial class Vehiculo
{
    public int VehiId { get; set; }

    public int TipoVehiculoFk { get; set; }

    public string VehiPlaca { get; set; } = null!;

    public string? VehiMarca { get; set; }

    public string? VehiModelo { get; set; }

    public string? VehiColor { get; set; }

    public bool? VehiEstado { get; set; }

    public virtual ICollection<RegistrosEstacionamiento> RegistrosEstacionamientos { get; set; } = new List<RegistrosEstacionamiento>();

    public virtual TipoVehiculo TipoVehiculoFkNavigation { get; set; } = null!;
}
