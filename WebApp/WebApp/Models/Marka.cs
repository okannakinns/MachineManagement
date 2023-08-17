using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class Marka
{
    public int Id { get; set; }

    public string Adi { get; set; } = null!;

    public DateTime? OlusturulmaTarihi { get; set; }

    public DateTime? GuncellenmeTarihi { get; set; }

    public int? Durumu { get; set; }

    public virtual ICollection<Makine> Makines { get; set; } = new List<Makine>();
}
