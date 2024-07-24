using System;
using System.Collections.Generic;

namespace oracleDataAcess.Models;

public partial class Dlleventum
{
    public decimal Iddventa { get; set; }

    public decimal? Idprod { get; set; }

    public decimal? Idvta { get; set; }

    public decimal? Cantidad { get; set; }

    public decimal? Subtotal { get; set; }

    public virtual Producto? IdprodNavigation { get; set; }

    public virtual Ventum? IdvtaNavigation { get; set; }
}
