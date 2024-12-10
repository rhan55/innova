using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Areas.E.Models.Dto
{
    public class ETicaretSepetDto
    {

        public class ETicaretSepetListeSorguDto
        {
            private string _CariID;
            private string _UyelikID;

            public string CariID { get { return _CariID ?? ""; } set { _CariID = value; } }
            public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        }

        public class ETicaretSepetListeSonucDto
        {
            private string _ID;
            private string _CariID;
            private string _UyelikID;
            private string _KayitTarihi;
            private string _StokID;
            private string _OlcuBirimi;
            private decimal _Miktar;
            private decimal _Fiyat;
            private string _DovizBirimi;
            private bool _Silindi;
            private string _Kod;
            private string _Isim;
            private string _Resim1;

            public string ID { get { return _ID ?? ""; } set { _ID = value; } }
            public string CariID { get { return _CariID ?? ""; } set { _CariID = value; } }
            public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
            public string StokID { get { return _StokID ?? ""; } set { _StokID = value; } }
            public string KayitTarihi { get { return _KayitTarihi ?? ""; } set { _KayitTarihi = value; } }
            public string OlcuBirimi { get { return _OlcuBirimi ?? ""; } set { _OlcuBirimi = value; } }
            public decimal Miktar { get { return _Miktar; } set { _Miktar = value; } }
            public decimal Fiyat { get { return _Fiyat; } set { _Fiyat = value; } }
            public string DovizBirimi { get { return _DovizBirimi ?? ""; } set { _DovizBirimi = value; } }
            public bool Silindi { get { return _Silindi; } set { _Silindi = value; } }
            public string Kod { get { return _Kod ?? ""; } set { _Kod = value; } }
            public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
            public string Resim1 { get { return _Resim1 ?? ""; } set { _Resim1 = value; } }
        }

        public class ETicaretSepetEkleDto
        {
            private string _CariID;
            private string _UyelikID;
            private string _StokID;
            private string _OlcuBirimi;
            private decimal _Miktar;
            private decimal _Fiyat;
            private string _DovizBirimi;

            public string CariID { get { return _CariID ?? ""; } set { _CariID = value; } }
            public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
            public string StokID { get { return _StokID ?? ""; } set { _StokID = value; } }
            public string OlcuBirimi { get { return _OlcuBirimi ?? ""; } set { _OlcuBirimi = value; } }
            public decimal Miktar { get { return _Miktar; } set { _Miktar = value; } }
            public decimal Fiyat { get { return _Fiyat; } set { _Fiyat = value; } }
            public string DovizBirimi { get { return _DovizBirimi ?? ""; } set { _DovizBirimi = value; } }
        }
    }
}