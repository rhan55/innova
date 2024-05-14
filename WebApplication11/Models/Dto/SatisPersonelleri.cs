using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class SatisPersoneliDto
    {
        private string _ID;
        private string _UyelikID;
        private string _Isim;
        private string _Aciklama1;
        private string _Aciklama2;
        private string _KullaniciID;

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
        public string Aciklama1 { get { return _Aciklama1 ?? ""; } set { _Aciklama1 = value; } }
        public string Aciklama2 { get { return _Aciklama2 ?? ""; } set { _Aciklama2 = value; } }
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
      

    }
}

