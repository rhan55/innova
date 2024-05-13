using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class CariKisiDto
    {
        private string _ID;
        private string _CariID;
        private string _UyelikID;
        private string _Isim;
        private string _Email;
        private string _Gorev;
        private string _Telefon;
        private bool _Aktif;
        private string _KullaniciID;

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string CariID { get { return _CariID ?? ""; } set { _CariID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
        public string Email { get { return _Email ?? ""; } set { _Email = value; } }
        public string Gorev { get { return _Gorev ?? ""; } set { _Gorev = value; } }
        public string Telefon { get { return _Telefon ?? ""; } set { _Telefon = value; } }
        public bool Aktif { get { return _Aktif == null ? false : _Aktif; } set { _Aktif = value; } }
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }



    }
}

