using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class Fotograf
    {
        public int Id { get; set; }

        public int? IsId { get; set; }    
        public string? Adi { get; set; }
        public string? Yolu { get; set; }
        public DateTime? OlusturulmaTarihi { get; set; }
        public DateTime? GuncellenmeTarihi { get; set; }
        public int? Durumu { get; set; }

        public virtual Is Job { get; set; }
    }
}
