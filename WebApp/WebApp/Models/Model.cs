using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class Model
{
    public int Id { get; set; }

    public int? MarkaId { get; set; }
    public string Adi { get; set; }

    public DateTime? OlusturulmaTarihi { get; set; }

    public DateTime? GuncellenmeTarihi { get; set; }

    public int? Durumu { get; set; }
    public virtual Marka Marka {get;set;}

    public virtual ICollection<Makine> Makines { get; set; } = new List<Makine>();
}