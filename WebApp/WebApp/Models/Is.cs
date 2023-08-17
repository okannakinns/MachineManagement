using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class Is
{
    public int Id { get; set; }

    public string? Adi { get; set; }

   
    public int MusteriId { get; set; }

    public DateTime? BaslangicTarihi { get; set; }

    public DateTime? BitisTarihi { get; set; }
    public string? KalanZaman { get; set; }
    public DateTime? OlusturulmaTarihi { get; set; }

    public DateTime? GuncellenmeTarihi { get; set; }

    public int? Durumu { get; set; }

    
    public virtual Musteri Musteri { get; set; }

}

