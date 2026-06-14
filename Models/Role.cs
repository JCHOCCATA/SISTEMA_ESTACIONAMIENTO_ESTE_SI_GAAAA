using System;
using System.Collections.Generic;

namespace SistemaEstacionamiento.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleNombreRol { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
