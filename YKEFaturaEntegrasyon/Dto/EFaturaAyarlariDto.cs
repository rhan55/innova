using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKEFaturaEntegrasyon.Dto
{
    public class EFaturaAyarlariDto
    {
        public string ServiceUrl { get; set; } = string.Empty;
        public string KullaniciAdi { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
        public string GrupKodu { get; set; } = string.Empty;
    }
}