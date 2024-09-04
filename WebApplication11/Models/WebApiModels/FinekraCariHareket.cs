using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.WebApiModels
{
    public class FinekraCariHareket
    {
        public string Banka { get; set; }
        public string DekontSeriNo { get; set; }
        public decimal Tutar { get; set; }
        public DateTime ValorTarihi { get; set; }
        public DateTime Tarih { get; set; }
        public string HesapKodu { get; set; }
        public string VirmanHesapKodu { get; set; }

        public string CariKodu { get; set; }
        public string BorcAlacak { get; set; }
        public string IslemTipi { get; set; }
        public string sorumlulukMerkezi { get; set; }
        public string Aciklama { get; set; }
        public string EvrakNo { get; set; }
        public string Durum { get; set; }
        public string SirketKodu { get; set; }
        public string SubeKodu { get; set; }
        public string ProjeKodu { get; set; }
        public string ReferansKodu { get; set; }
        public string DovizTipi { get; set; }
        public decimal DovizKuru { get; set; }
        public decimal DovizTutari { get; set; }
        public string PlasiyerKodu { get; set; }


        public string Banka2 { get; set; }
        public string SubeKodu2 { get; set; }
        public decimal Tutar2 { get; set; }
        public string DovizTipi2 { get; set; }
        public decimal DovizTutari2 { get; set; }
        public string Aciklama2 { get; set; }
        public string HareketTuru { get; set; }

        public DateTime GirisTarihi { get; set; }
        public DateTime VadeTarihi { get; set; }
        public DateTime OdemeTarihi { get; set; }
    }
}