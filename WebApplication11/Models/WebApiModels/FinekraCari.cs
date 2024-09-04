using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.WebApiModels
{
    public class FinekraCari
    {
        public int ID { get; set; }
        public string ERPKodu { get; set; }
        public string CariKodu { get; set; }
        public string AnaCariKodu { get; set; }
        public DateTime Tarih { get; set; }

        public string CariAdi { get; set; }
        public string CariAdi2 { get; set; }
        public string VergiDairesi { get; set; }
        public string VergiNumarasi { get; set; }
        public string TCKimlikNo { get; set; }
        public string Telefon { get; set; }
        public string CepTelefonu { get; set; }
        public string FaxNumarasi { get; set; }
        public string Adres { get; set; }
        public string CariTipi { get; set; }
        public string SaticiKodu { get; set; }
        public string CariSinifi { get; set; }
        public string GrupKodu { get; set; }
        public string IsletmeKodu { get; set; }
        public string SubeKodu { get; set; }
        public string PlasiyerKodu { get; set; }
        public string VeritabaniAdi { get; set; }
        public decimal Borc { get; set; }
        public decimal Alacak { get; set; }
        public decimal Bakiye { get; set; }
        public string MuhasebeKodu { get; set; }
        public string ReferansKodu { get; set; }
        public string ProjeKodu { get; set; }
    }
}