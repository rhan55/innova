using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class BelgeKalemDto
    {
        private string _ID;
        private string _BelgeID;
        private string _StokID;
        private string _StokKodu;
        private string _StokAdi;
        private string _OlcuBirimi;
        private string _Seri;
        private decimal _Miktar;
        private decimal _Fiyat;
        private decimal _Iskonto;
        private decimal _Tutar;
        private decimal _KdvOrani;
        private decimal _IskontoOrani1;
        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string BelgeID { get { return _BelgeID ?? ""; } set { _BelgeID = value; } }
        public string StokID { get { return _StokID ?? ""; } set { _StokID = value; } }
        public string StokKodu { get { return _StokKodu ?? ""; } set { _StokKodu = value; } }
        public string StokAdi { get { return _StokAdi ?? ""; } set { _StokAdi = value; } }
        public string OlcuBirimi { get { return _OlcuBirimi ?? ""; } set { _OlcuBirimi = value; } }
        public string Seri { get { return _Seri ?? ""; } set { _Seri = value; } }
        public decimal Miktar { get { return _Miktar == null ? 0 : _Miktar; } set { _Miktar = value; } }
        public decimal Fiyat { get { return _Fiyat == null ? 0 : _Fiyat; } set { _Fiyat = value; } }
        public decimal Iskonto { get { return _Iskonto == null ? 0 : _Iskonto; } set { _Iskonto = value; } }
        public decimal Tutar { get { return _Tutar == null ? 0 : _Tutar; } set { _Tutar = value; } }
        public decimal KdvOrani { get { return _KdvOrani == null ? 0 : _KdvOrani; } set { _KdvOrani = value; } }
        public decimal IskontoOrani1 { get { return _IskontoOrani1 == null ? 0 : _IskontoOrani1; } set {  _IskontoOrani1 = value; } }
    }
}