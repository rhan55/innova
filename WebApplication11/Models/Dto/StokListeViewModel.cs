using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace YKPortal.Models.Dto
{
    public class StokListeViewModel
    {
        public DataTable Stoklar{ get; set; }
        public bool Sil { get; set; }
        public bool Duzenle { get; set; }
    }

    public class StokSayim
    {
        public string ID { get; set; }
        public string UyelikID { get; set; }
        public string Firma { get; set; }
        public string Sube { get; set; }
        public string Depo { get; set; }
        public string Barkod { get; set; }
        public string StokID { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public decimal Miktar { get; set; }
        public bool Aktarildi { get; set; }
        public DateTime Tarih { get; set; }
    }
}