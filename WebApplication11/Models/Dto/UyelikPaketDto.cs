using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class UyelikPaketDto
    {
        private string _ID;
        private string _Isim;
        private string _Ay;
        private string _Tutar;
        private string _ResimUrl;
        private string _Aciklama;

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
        public string Ay { get { return _Ay ?? ""; } set { _Ay = value; } }
        public string Tutar { get { return _Tutar ?? ""; } set { _Tutar = value; } }
        public string ResimUrl { get { return _ResimUrl ?? ""; } set { _ResimUrl = value; } }
        public string Aciklama { get { return _Aciklama ?? ""; } set { _Aciklama = value; } }
    }
}