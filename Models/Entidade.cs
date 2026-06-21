using System;
using System.Collections.Generic;

namespace SistemaEstacionamiento.Models;

public partial class Entidade
{
    public int EntiId { get; set; }

    public int TipoDocumentoFk { get; set; }

    public string EntiRazonSocial { get; set; } = null!;

    public string EntiNroDocumento { get; set; } = null!;

    public string EntiEstado { get; set; } = null!;

    public DateTime? EntiFechaCreacion { get; set; }

    public string? EntiUsuarioCreacion { get; set; }

    public DateTime? EntiFechaModificacion { get; set; }

    public string? EntiUsuarioModificacion { get; set; }

    public virtual TipoDocumento TipoDocumentoFkNavigation { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
