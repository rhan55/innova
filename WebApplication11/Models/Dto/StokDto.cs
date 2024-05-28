using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class StokDto
    {
        private string _ID; 
        private string _UyelikID;
        private bool _Aktif;
        private DateTime _KayitTarihi;
        private string _Kod;
        private string _Isim;



        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public bool Aktif { get { return _Aktif == null ? false : _Aktif; } set { _Aktif = value; } }
        public DateTime KayitTarihi { get { return _KayitTarihi == null ? DateTime.Now :  _KayitTarihi; } set { _KayitTarihi = value; } }
        public string Kod { get { return _Kod ?? ""; } set { _Kod = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }

    }
}

