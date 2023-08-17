using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class Makine
{
    public int Id { get; set; }

    public string? Adi { get; set; }

    public int? MakineTipId { get; set; }
    public int? MarkaId { get; set; }
    public int? ModelId { get; set; }

    public string? Plaka { get; set; }
    public int Km { get; set; }
    public int MakineDurumId { get; set; }

    public int? ToplamCalismaSaati { get; set; }

    public DateTime? OlusturulmaTarihi { get; set; }

    public DateTime? GuncellenmeTarihi { get; set; }

    public int? Durumu { get; set; }

  

    public virtual MakineDurum MakineDurum { get; set; } 

    public virtual MakineTip MakineTip { get; set; }
    public virtual Marka Marka { get; set; }
    public virtual Model Model { get; set; }


    public virtual ICollection<FisIslemleri> Sayacs { get; set; } = new List<FisIslemleri>();
}
