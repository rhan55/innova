using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class PersonelDto
    {
        private string _ID;
        private string _UyelikID;
        private string _Isim;
        private string _Email;
        private string _Telefon;
        private string _KullaniciID;

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
        public string Email { get { return _Email ?? ""; } set { _Email = value; } }
        public string Telefon { get { return _Telefon ?? ""; } set { _Telefon = value; } }
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
    }
}