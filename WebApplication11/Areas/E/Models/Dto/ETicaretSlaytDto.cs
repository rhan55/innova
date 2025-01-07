using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Areas.E.Models.Dto
{
    public class ETicaretSlaytDto
    {
        private string _SlaytID;
        private string _ResimYolu;
        private string _Link;
        private string _Text;
        private bool _Durum;
        public string _OlusturulmaTarihi;
        public int _Siralama;
        public string _Tip;
        public string _TipDeger;


        public string SlaytID { get { return _SlaytID ?? ""; } set { _SlaytID = value; } }
        public string ResimYolu { get { return _ResimYolu ?? ""; } set { _ResimYolu = value; } }
        public string Link { get { return _Link ?? ""; } set { _Link = value; } }
        public string Text { get { return _Text ?? ""; } set { _Text = value; } }
        public bool Aktif { get { return _Durum; } set { _Durum = value; } }        
        public int Siralama { get { return _Siralama; } set { _Siralama = value; } }
        public string OlusturulmaTarihi { get { return _OlusturulmaTarihi ?? ""; } set { _OlusturulmaTarihi = value; } }
        public string Tip { get { return _Tip ?? ""; } set { _Tip = value; } }
        public string TipDeger { get { return _TipDeger ?? ""; } set { _TipDeger = value; } }

    }
}


