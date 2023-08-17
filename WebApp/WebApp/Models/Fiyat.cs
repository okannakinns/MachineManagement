using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class Fiyat
{
    public int Id { get; set; }

    public string? Adi { get; set; }
    public int MakineTipId { get; set; }

    public decimal? Saatlik { get; set; }

    public decimal? Gunluk { get; set; }

    public DateTime? OlusturulmaTarihi { get; set; }

    public DateTime? GuncellenmeTarihi { get; set; }

    public int? Durumu { get; set; }

    public virtual MakineTip MakineTip { get; set; } = null!;
 
    public virtual ICollection<FisIslemleri> Sayacs { get; set; } = new List<FisIslemleri>();
}
