using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class GrupKoduDto
    {
        private string _ID;
        private string _UyelikID;
        private string _Kod;
        private string _Deger;
        private bool _Aktif;

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string Kod { get { return _Kod ?? ""; } set { _Kod = value; } }
        public string Deger { get { return _Deger ?? ""; } set { _Deger = value; } }
        public bool Aktif { get { return _Aktif == null ? false : _Aktif; } set { _Aktif = value; } }

        public override string ToString()
        {
            return Deger;
        }

    }
}

