using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class SubeDto
    {
            private string _ID;
            private string _UyelikID;
            private string _KullaniciID;
            private string _Kod;
            private string _Isim;
            private string _Adres;
            private string _Telefon;
            
            public string ID { get { return _ID ?? ""; } set { _ID = value; } }
            public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
            public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
            public string Kod { get { return _Kod ?? ""; } set { _Kod = value; } }
            public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
            public string Adres { get { return _Adres ?? ""; } set { _Adres = value; } }
            public string Telefon { get { return _Telefon ?? ""; } set { _Telefon = value; } }
                         
    } 
}
