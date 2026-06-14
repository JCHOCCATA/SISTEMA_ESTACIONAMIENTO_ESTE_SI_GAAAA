using System;
using System.Collections.Generic;

namespace SistemaEstacionamiento.Models;

public partial class TipoDocumento
{
    public int TidoId { get; set; }

    public string TidoNombreDoc { get; set; } = null!;

    public virtual ICollection<Entidade> Entidades { get; set; } = new List<Entidade>();
}
