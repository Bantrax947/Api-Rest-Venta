using System;
using System.Collections.Generic;

namespace oracleDataAcess.Models;

public partial class Usuario
{
    public decimal Idusu { get; set; }

    public string? Username { get; set; }

    public string? Pass { get; set; }

    public string? Rol { get; set; }
}
