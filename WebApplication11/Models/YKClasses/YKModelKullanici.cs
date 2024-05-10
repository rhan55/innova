using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.YKClasses
{
    public class YKModelKullanici
    {
        public string KullaniciID { get; set; }
        public string Isim { get; set; }
        public string KullaniciAdi { get; set; }
        public string Parola { get; set; }
        public string UyelikIsim { get; set; }
        public string UyelikID { get; set; }
        public string Resim { get; set; }
    }
}