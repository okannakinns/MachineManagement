using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class MakineTip
{
    public int Id { get; set; }

    public string? Adi { get; set; }

    public DateTime? OlusturulmaTarihi { get; set; }

    public DateTime? GuncellenmeTarihi { get; set; }

    public int? Durumu { get; set; }

    public virtual ICollection<Fiyat> Fiyats { get; set; } = new List<Fiyat>();

    public virtual ICollection<Makine> Makines { get; set; } = new List<Makine>();
}
