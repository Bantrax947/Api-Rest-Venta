using System;
using System.Collections.Generic;

namespace oracleDataAcess.Models;

public partial class Cliente
{
    public decimal IdClient { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public decimal? Ci { get; set; }

    public decimal? Edad { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
