using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Razor.Parser.SyntaxTree;
using System.Xml.Linq;

namespace YKPortal.Models.Dto
{
    public class POSAPIDto
    {

        private string _UyelikID;
        private string _KullaniciID;
        private string _Uygulama;
        private string _Tutar;
        private string _UzatilacakAy;
        private string _Durumu;
        private string _OrderID;
        private string _KrediKartIsim;
        private string _KrediKartNo;
        private string _KrediKartSonKullanim;
        private string _KrediKartCVV;
        private string _SonucKodu;
        private string _SonucAciklama;
        

     
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID  = value; } }
        public string Uygulama { get { return _Uygulama ?? ""; } set { _Uygulama = value; } }
        public string Tutar { get { return _Tutar ?? ""; } set { _Tutar = value; } }
        public string UzatilacakAy { get { return _UzatilacakAy ?? ""; } set { _UzatilacakAy = value; } }
        public string Durumu { get { return _Durumu ?? ""; } set { _Durumu = value; } }
        public string OrderID { get { return _OrderID ?? ""; } set { _OrderID = value; } }
        public string KrediKartIsim { get { return _KrediKartIsim ?? ""; } set { _KrediKartIsim = value; } }
        public string KrediKartNo { get { return _KrediKartNo ?? ""; } set { _KrediKartNo = value; } }
        public string KrediKartSonKullanim { get { return _KrediKartSonKullanim ?? ""; } set { _KrediKartSonKullanim = value; } }
        public string KrediKartCVV { get { return _KrediKartCVV ?? ""; } set { _KrediKartCVV = value; } }
        public string SonucKodu { get { return _SonucKodu ?? ""; } set { _SonucKodu = value; } }
        public string SonucAciklama { get { return _SonucAciklama ?? ""; } set { _SonucAciklama = value; } }
      

    }
}
