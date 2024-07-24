using System;
using System.Collections.Generic;

namespace oracleDataAcess.Models;

public partial class Producto
{
    public decimal Idproducto { get; set; }

    public string? Nombreprod { get; set; }

    public decimal? Cantidad { get; set; }

    public decimal? Preciounit { get; set; }

    public virtual ICollection<Dlleventum> Dlleventa { get; set; } = new List<Dlleventum>();
}
