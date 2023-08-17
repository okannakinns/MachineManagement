using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class Musteri
{
    public int Id { get; set; }

    public string? AdiSoyadi { get; set; }

    public string? Telefon { get; set; }

    public string? Aciklama { get; set; }

    public DateTime? OlusturulmaTarihi { get; set; }

    public DateTime? GuncellenmeTarihi { get; set; }

    public string? Email { get; set; }
    public int? Durumu { get; set; }

    
}
