using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YKPortal.Models.Dto
{
    public class StokNotDto
    {
            private string _ID;
            private string _UyelikID;
            private string _KullaniciID;
            private string _StokID;
            private string _Aciklama;

            
            public string ID { get { return _ID ?? ""; } set { _ID = value; } }
            public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
            public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
            public string StokID { get { return _StokID ?? ""; } set { _StokID = value; } }
            public string Aciklama { get { return _Aciklama ?? ""; } set { _Aciklama = value; } }
          
    } 
}
 