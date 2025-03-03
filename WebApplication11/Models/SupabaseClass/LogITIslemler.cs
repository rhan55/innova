using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.SupabaseClass
{
    public class LogItIslemler
    {
        public int Id { get; set; } // Otomatik artan ID (Opsiyonel)
        public DateTime created_at { get; set; }
        public string Sirket { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public string Modul { get; set; } = string.Empty;
        public string Islem { get; set; } = string.Empty;
        public string Baslik { get; set; } = string.Empty;
        public string Deger { get; set; } = string.Empty;
        public string Kullanici { get; set; } = string.Empty;
        public string IP { get; set; } = string.Empty;
    }
}