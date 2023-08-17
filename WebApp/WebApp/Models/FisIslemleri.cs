using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class FisIslemleri
{
    public int Id { get; set; }

    public int MakineId { get; set; }

    public int IsId { get; set; }
    public int SonKm { get; set; }

    public DateTime? BaslangicTarihi { get; set; }

    public DateTime? BitisTarihi { get; set; }

    public int? FiyatId { get; set; }

    public string CalismaSuresi { get; set; }

    public decimal? Ucret { get; set; }

    public DateTime? OlusturulmaTarihi { get; set; }

    public DateTime? GuncellenmeTarihi { get; set; }

    public int? Durumu { get; set; }

    public virtual Fiyat? Fiyat { get; set; }
    public virtual Is? Is { get; set; }


    public virtual Makine Makine { get; set; } = null!;
}
