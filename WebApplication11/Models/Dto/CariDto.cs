using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication11.Models.Dto
{
    public class CariDto
    {
        public string ID { get; set; }
        public string UyelikID { get; set; }
        public bool Aktif { get; set; } = false;
        public string KayitTarihi { get; set; }
        public string Kod { get; set; } = string.Empty;
        public string Isim { get; set; } = string.Empty;
        public string Unvan { get; set; } = string.Empty;
        public string Adres { get; set; }
        public string Ilce { get; set; }
        public string Il { get; set; }
        public string Ulke { get; set; }
        public string Bolge { get; set; }
        public string TCKimlikNo { get; set; } = string.Empty;
        public string VergiDairesi { get; set; }
        public string VergiNumarasi { get; set; } = string.Empty;
        public string PostaKodu { get; set; }
        public bool Alici { get; set; } = false;
        public bool Satici { get; set; } = false;
        public bool Personel { get; set; } = false;
        public string Telefon1 { get; set; }
        public string Telefon2 { get; set; }
        public string EMail { get; set; }
        public string Faks { get; set; }
        public string CepTelefonu { get; set; } = string.Empty;
        public string WebSite { get; set; }
        public string GrupKodu1ID { get; set; }
        public string GrupKodu2ID { get; set; }
        public string GrupKodu3ID { get; set; }
        public string GrupKodu4ID { get; set; }
        public string GrupKodu5ID { get; set; }
        public string GrupKodu6ID { get; set; }
        public string MuhasebeKodu { get; set; }
        public bool Kilitli { get; set; } = false;
        public string KilitAciklamasi { get; set; }
        public string DovizID { get; set; }
        public int VadeGunu { get; set; }
        public decimal Iskonto1 { get; set; }
        public int ListeFiyat { get; set; }
        public string Aciklama1 { get; set; }
        public string Aciklama2 { get; set; }
        public string Aciklama3 { get; set; }

        public string Aciklama4 { get; set; }
        public string Aciklama5 { get; set; }
        public string Aciklama6 { get; set; }
        public bool LimitAsimindaUyar { get; set; }
        public bool LimitAsimindaDurdur { get; set; } = false;
        public bool CekSenetRiski { get; set; } = false;
        public decimal Limit { get; set; }
        public bool ServisPersoneli { get; set; } = false;
        public string KullaniciAdi { get; set; }
        public string Parola { get; set; }
        public string RiskAciklama { get; set; }
        public string PlasiyerID { get; set; }
        public string AnaCariID { get; set; }
        public string TeslimCariID { get; set; }
        public string KullaniciID { get; set; }
       
    }
}

