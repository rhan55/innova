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
        private DateTime _BitisGünü;
        private DateTime _UyelikBitisTarihi;
        private string _KullaniciAdi;
        private string _Parola;
        private string _UyelikIsim;
        private string _UyelikID;
        private string _Resim;
        private string _Uygulama;
        private string _Uygulama_Sube_Kodu = "0";
        private string _Uygulama_Depo_Kodu = "0";
        private string _Uygulama_Db;
        private string _UyelikBitisGunu = "0";

        public string ID { get; set; }
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
        public DateTime BitisGünü { get { return _BitisGünü == null ? DateTime.Today : _BitisGünü; } set { _BitisGünü = value; } }
        public DateTime UyelikBitisTarihi { get { return _UyelikBitisTarihi == null ? DateTime.Today : _UyelikBitisTarihi; } set { _UyelikBitisTarihi = value; } }
        public string KullaniciAdi { get { return _KullaniciAdi ?? ""; } set { _KullaniciAdi = value; } }
        public string Parola { get { return _Parola ?? ""; } set { _Parola = value; } }
        public string UyelikIsim { get { return _UyelikIsim ?? ""; } set { _UyelikIsim = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string Resim { get { return _Resim ?? ""; } set { _Resim = value; } }
        public string Uygulama { get { return _Uygulama ?? ""; } set { _Uygulama = value; } }
        public string Uygulama_Db { get { return _Uygulama_Db ?? ""; } set { _Uygulama_Db = value; } }
        public string Uygulama_Sube_Kodu { get { return _Uygulama_Sube_Kodu ?? "0"; } set { _Uygulama_Sube_Kodu = value; } }
        public string Uygulama_Depo_Kodu { get { return _Uygulama_Depo_Kodu ?? "0"; } set { _Uygulama_Depo_Kodu = value; } }

        public string UyelikBitisGunu { get { return _UyelikBitisGunu ?? "0"; } set { _UyelikBitisGunu = value; } }

    }
}