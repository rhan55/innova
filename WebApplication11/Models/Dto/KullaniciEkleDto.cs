using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    
    
    public class KullaniciEkleDto
    {
        private string _ID;
        private string _UyelikID;
        private string _KullaniciAdi;
        private string _Parola;
        private string _Ad;
        private string _Soyad;
        private bool _Aktif;
        private string _Telefon;
        private string _Adres;
        private string _Il;
        private string _Ilce;
        private string _Aciklama1;
        private string _Aciklama2;
        private string _Aciklama3;
        private string _Kullanici;
        private string _Resim;
        private bool _Onay;
        private string _YeniMesaj;
        private string _SonMesajTarihi;
        private string _SonMesajIcerigi;

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; }}
        [Required(ErrorMessage = "Kullanıcı Adını Giriniz.")]
        public string KullaniciAdi { get { return _KullaniciAdi ?? ""; } set { _KullaniciAdi = value; } }
        [Required(ErrorMessage= "Kullanıcı Parolasını Giriniz")]
        public string Parola { get { return _Parola ?? ""; } set { _Parola = value; } }
        [Required(ErrorMessage = "Ad Giriniz")]
        public string Ad { get { return _Ad ?? ""; } set { _Ad = value; } }
        [Required(ErrorMessage = "Soyad Giriniz")]
        public string Soyad { get { return _Soyad ?? ""; } set { _Soyad = value; } }
        public bool Aktif { get { return _Aktif == null ? false : _Aktif; } set { _Aktif = value; } }
        public string Telefon { get { return _Telefon ?? ""; } set { _Telefon = value; } }
        public string Adres { get { return _Adres ?? ""; } set { _Adres = value; } }
        public string Il { get { return _Il ?? ""; } set { _Il = value; } }
        public string Ilce { get { return _Ilce ?? ""; } set { _Ilce = value; } }
        public string Aciklama1 { get { return _Aciklama1 ?? ""; } set { _Aciklama1 = value; } }
        public string Aciklama2 { get { return _Aciklama2 ?? ""; } set { _Aciklama2 = value; } }
        public string Aciklama3 { get { return _Aciklama3 ?? ""; } set { _Aciklama3 = value; } }
        public string Kullanici { get { return _Kullanici ?? ""; } set { _Kullanici = value; } }
        public string Resim { get { return _Resim ?? ""; } set { _Resim = value; } }
        public bool Onay { get { return _Onay == null ? false : _Onay ; } set { _Onay = value; } }
        public string YeniMesaj { get { return _YeniMesaj ?? ""; } set { _YeniMesaj = value; } }
        public string SonMesajTarihi { get { return _SonMesajTarihi ?? ""; } set { _SonMesajTarihi = value; } }
        public string SonMesajIcerigi { get { return _SonMesajIcerigi ?? ""; } set { _SonMesajIcerigi = value; } }

    }
}

