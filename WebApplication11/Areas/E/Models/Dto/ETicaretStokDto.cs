using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YKPortal.Areas.E.Models.Dto
{
    public class ETicaretStokDto
    {
        public class ETicaretStokSorguDto
        {

            private string _StokID;
            private string _CariID;
            private string _UyelikID;
            private string _Kategori1;
            private string _Kategori2;
            private string _Kategori3;
            private string _Kategori4;
            private string _Kategori5;
            private string _Kategori6;
            private string _AranacakKelime;
            private int _Sayfa;

            public string StokID { get { return _StokID ?? ""; } set { _StokID = value; } }
            public string CariID { get { return _CariID ?? ""; } set { _CariID = value; } }
            public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
            public string Kategori1 { get { return _Kategori1 ?? ""; } set { _Kategori1 = value; } }
            public string Kategori2 { get { return _Kategori2 ?? ""; } set { _Kategori2 = value; } }
            public string Kategori3 { get { return _Kategori3 ?? ""; } set { _Kategori3 = value; } }
            public string Kategori4 { get { return _Kategori4 ?? ""; } set { _Kategori4 = value; } }
            public string Kategori5 { get { return _Kategori5 ?? ""; } set { _Kategori5 = value; } }
            public string Kategori6 { get { return _Kategori6 ?? ""; } set { _Kategori6 = value; } }
            public string AranacakKelime { get { return _AranacakKelime ?? ""; } set { _AranacakKelime = value; } }
            public int Sayfa { get { return _Sayfa; } set { _Sayfa = value; } }
        }

        public class ETicaretStokSonucDto
        {
            private string _StokID;
            private string _UyelikID;
            private string _Kod;
            private string _Isim;
            private string _Aciklama;
            private string _Barkod;
            private string _OlcuBirimi;
            private string _KdvSatis;
            private decimal _IskontoSatis1;
            private string _Marka;
            private string _Model;
            private string _Renk;
            private string _Beden;
            private string _Kalite;
            private string _UreticiFirma;
            private decimal _Fiyat;
            private string _Resim1;
            private string _Resim2;

            public ETicaretStokSorguDto Kategoriler { get; set; } // Diğer DTO ile ilişki
            public string StokID { get { return _StokID ?? ""; } set { _StokID = value; } }
            public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
            public string Kod { get { return _Kod ?? ""; } set { _Kod = value; } }
            public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
            public string Aciklama { get { return _Aciklama ?? ""; } set { _Aciklama = value; } }
            public string Barkod { get { return _Barkod ?? ""; } set { _Barkod = value; } }
            public string OlcuBirimi { get { return _OlcuBirimi ?? ""; } set { _OlcuBirimi = value; } }
            public string KdvSatis { get { return _KdvSatis ?? ""; } set { _KdvSatis = value; } }
            public decimal IskontoSatis1 { get { return _IskontoSatis1; } set { _IskontoSatis1 = value; } }
          
            public string Marka { get { return _Marka ?? ""; } set { _Marka = value; } }
            public string Model { get { return _Model ?? ""; } set { _Model = value; } }
            public string Renk { get { return _Renk ?? ""; } set { _Renk = value; } }
            public string Beden { get { return _Beden ?? ""; } set { _Beden = value; } }
            public string Kalite { get { return _Kalite ?? ""; } set { _Kalite = value; } }
            public string UreticiFirma { get { return _UreticiFirma ?? ""; } set { _UreticiFirma = value; } }
            public decimal Fiyat { get { return _Fiyat; } set { _Fiyat = value; } }
            public string Resim2 { get { return _Resim2 ?? ""; } set { _Resim2 = value; } }
            public string Resim1 { get { return _Resim1 ?? ""; } set { _Resim1 = value; } }
        }


    }
}
