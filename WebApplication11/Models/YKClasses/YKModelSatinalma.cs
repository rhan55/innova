using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.YKClasses
{
    public class YKModelSatinalma
    {
            public string ID { get; set; }
            public string SubeKodu { get; set; }
            public string Tarih { get; set; }
            public string Kod { get; set; }
            public string Isim { get; set; }
            public string OlcuBirimi { get; set; }
            public string CariKodu { get; set; }
            public string CariAdi { get; set; }
            public string Barkod { get; set; }
            public string Barkod2 { get; set; }
            public string StokKod { get; set; }
            public string StokIsim { get; set; }
            public string Fason { get; set; }
            public string Renk { get; set; }
            public string Beden { get; set; }
            public decimal Miktar { get; set; }
            public decimal ToplamMiktar { get; set; }
            public decimal Fiyat { get; set; }
            public decimal Kdv { get; set; }
            public decimal Iskonto { get; set; }
            public decimal Tutar { get; set; }
            public string Aciklama1 { get; set; }
            public string Aciklama2 { get; set; }
            public string Aciklama3 { get; set; }
            public string Kullanici { get; set; }
            public string KullaniciKodu { get; set; }
            public string KullaniciAdi { get; set; }

            public string DosyaAdi { get; set; }
            public string DosyaLinki { get; set; }
            public string DosyaID { get; set; }

            public string SonAlisCarisi { get; set; }
            public string SonAlisTarihi { get; set; }
            public string SonAlisFiyati { get; set; }
            public string SonAlisSubesi { get; set; }
        
    }
}