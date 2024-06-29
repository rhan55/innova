using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Razor.Parser.SyntaxTree;

namespace YKPortal.Models.Dto
{
    public class CariHareketDto
    {
 
        private string _ID;
        private string _UyelikID;
        private string _CariID;
        private string _Kullanici;
        private DateTime _Tarih;
        private string _Aciklama;
        private DateTime _VadeTarihi;
        private string _BelgeNo;
        private string _HareketTipi;
        private string _GC;
        private decimal _Tutar;
        private string _DovizTipi;
        private decimal _Kur;
        private decimal _DovizTutar;
        private string _PlasiyerID;
        private string _BaglantiID;
        private string _Baglanti;
        private string _GrupKodu1ID;
        private string _GrupKodu2ID;
        private string _BaslangicTarihi;
        private string _BitisTarihi;

        


        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string CariID { get { return _CariID ?? ""; } set { _CariID = value; } }
        public string Kullanici { get { return _Kullanici ?? ""; } set { _Kullanici = value; } }
        public DateTime Tarih { get { return _Tarih == null ? DateTime.Today : _Tarih; } set { _Tarih = value; } }
        public string Aciklama { get { return _Aciklama ?? ""; } set { _Aciklama = value; } }
        public DateTime VadeTarihi { get { return _VadeTarihi == null ? DateTime.Today : _VadeTarihi; } set { _VadeTarihi = value; } }
        public string BelgeNo { get { return _BelgeNo ?? ""; } set { _BelgeNo = value; } }
        public string HareketTipi { get { return _HareketTipi ?? ""; } set { _HareketTipi = value; } }
        public string GC { get { return _GC ?? ""; } set { _GC = value; } }
        public decimal Tutar { get { return _Tutar == null ? 0 : _Tutar; } set { _Tutar = value; } }
        public string DovizTipi { get { return _DovizTipi ?? ""; } set { _DovizTipi = value; } }
        public decimal Kur { get { return _Kur == null ? 0 : _Kur; } set { _Kur = value; } }
        public decimal DovizTutar { get { return _DovizTutar == null ? 0 : _DovizTutar; } set { _Kur = value; } }
        public string PlasiyerID { get { return _PlasiyerID ?? ""; } set { _PlasiyerID = value; } }
        public string BaglantiID { get { return _BaglantiID ?? ""; } set { _BaglantiID = value; } }
        public string Baglanti { get { return _Baglanti ?? ""; } set { _Baglanti = value; } }
        public string GrupKodu1ID { get { return _GrupKodu1ID ?? ""; } set { _GrupKodu1ID = value; } }
        public string GrupKodu2ID { get { return _GrupKodu2ID ?? ""; } set { _GrupKodu2ID = value; } }
        public string BaslangicTarihi { get { return _BaslangicTarihi ?? ""; } set { _BaslangicTarihi = value; } }
        public string BitisTarihi { get { return _BitisTarihi ?? ""; } set { _BitisTarihi = value; } }

    }
}

