using System;
using System.Collections.Generic;

namespace oracleDataAcess.Models;

public partial class Ventum
{
    public decimal Idventa { get; set; }

    public decimal? Idclient { get; set; }

    public DateTime? Fchaope { get; set; }

    public decimal? Montototal { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Dlleventum> Dlleventa { get; set; } = new List<Dlleventum>();

    public virtual Cliente? IdclientNavigation { get; set; }
}
