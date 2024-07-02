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
        private string _StandartID;
        private string _UyelikID;
        private string _Modul;
        private string _Isim;
        private string _Deger;
        private string _Tip;
        private string _Kategori;

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string StandartID { get { return _StandartID ?? ""; } set { _StandartID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
        public string Modul { get { return _Modul ?? ""; } set { _Modul = value; } }
        public string Deger { get { return _Deger ?? ""; } set { _Deger = value; } }
        public string Tip { get { return _Tip ?? ""; } set { _Tip = value; } }
        public string Kategori { get { return _Kategori ?? ""; } set { _Kategori = value; } }

    }
}

