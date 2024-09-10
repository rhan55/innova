using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class GorevDto
    {
        private string _ID;
        private string _UyelikID;
        private string _KullaniciID;
        private string _GorevTipiID;
        private string _Aciklama;
        private string _Durumu;
        private DateTime _BaslangicTarihi;
        private string _Periyot;
        private string[] _Kullanicilar;

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
        public string GorevTipiID { get { return _GorevTipiID ?? ""; } set { _GorevTipiID = value; } }
        public string Aciklama { get { return _Aciklama ?? ""; } set { _Aciklama = value; } }
        public string Durumu { get { return _Durumu ?? ""; } set { _Durumu = value; } }
        public DateTime BaslangicTarihi { get { return _BaslangicTarihi == null ? DateTime.Today : _BaslangicTarihi; } set { _BaslangicTarihi = value; } }
        public string Periyot { get { return _Periyot ?? ""; } set { _Periyot = value; } }
        public string[] Kullanicilar { get { return _Kullanicilar ?? null; } set { _Kullanicilar = value; } }
    }
}


