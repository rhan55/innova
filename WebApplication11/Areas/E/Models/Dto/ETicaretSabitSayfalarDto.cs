using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Areas.E.Models.Dto
{
    public class ETicaretSabitSayfalarDto
    {
        private string _SayfaID;
        private string _Icerik;
        private string _Ad;
        private string _UrlAd;
        private bool _Durum;
        private string _OlusturulmaTarihi;
        private string _GuncellemeTarihi;


        public string SayfaID { get { return _SayfaID ?? ""; } set { _SayfaID = value; } }
        public string Icerik { get { return _Icerik ?? ""; } set { _Icerik = value; } }
        public string Ad { get { return _Ad ?? ""; } set { _Ad = value; } }
        public string UrlAd { get { return _UrlAd ?? ""; } set { _UrlAd = value; } }
        public bool Durum { get { return _Durum; } set { _Durum = value; } }
        public string OlusturulmaTarihi { get { return _OlusturulmaTarihi ?? ""; } set { _OlusturulmaTarihi = value; } }
        public string GuncellemeTarihi { get { return _GuncellemeTarihi ?? ""; } set { _GuncellemeTarihi = value; } }


    }
}