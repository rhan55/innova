using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace YKPortal.Models.Dto
{
    public class DosyaDto
    {
        private string _ID;
        private string _UyelikID;
        private string _Modul;
        private string _KayitID;
        private string _Dosya;
        private string _KullaniciID;
   

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string Modul { get { return _Modul ?? ""; } set { _Modul = value; } }
        public string KayitID { get { return _KayitID ?? ""; } set { _KayitID = value; } }
        public string Dosya { get { return _Dosya ?? ""; } set { _Dosya = value; } }
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
    }
}
