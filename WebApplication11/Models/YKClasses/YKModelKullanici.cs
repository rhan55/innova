using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace YKPortal.Models.YKClasses
{
    public class YKModelKullanici
    {
        private string _KullaniciID;
        private string _Isim;
        private string _KullaniciAdi;
        private string _Parola;
        private string _UyelikIsim;
        private string _UyelikID;
        private string _Resim;

        public string ID { get; set; }
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
        public string KullaniciAdi { get { return _KullaniciAdi ?? ""; } set { _KullaniciAdi = value; } }
        public string Parola { get { return _Parola ?? ""; } set { _Parola = value; } }
        public string UyelikIsim { get { return _UyelikIsim ?? ""; } set { _UyelikIsim = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string Resim { get { return _Resim ?? ""; } set { _Resim = value; } }
 
    }
}