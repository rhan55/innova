using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class ParametreDto
    {
        private string _ID; 
        private string _UyelikID;
        private string _Modul;
        private string _Deger;
        private string _Isim;

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
        public string Modul { get { return _Modul ?? ""; } set { _Modul = value; } }
        public string Deger { get { return _Deger ?? ""; } set { _Deger = value; } }

    }
}

