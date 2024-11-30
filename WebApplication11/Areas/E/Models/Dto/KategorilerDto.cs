using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Areas.E.Models.Dto
{
    public class KategorilerDto
    {
        private string _CariID;
        private string _UyelikID;
        private string _Kategori1;
        private string _Kategori2;
        private string _Kategori3;
        private string _Kategori4;
        private string _Kategori5;
        private string _Kategori6;

        public string CariID { get { return _CariID ?? ""; } set { _CariID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string Kategori1{ get { return _Kategori1 ?? ""; } set { _Kategori1 = value; } }
        public string Kategori2 { get { return _Kategori2 ?? ""; } set { _Kategori2 = value; } }
        public string Kategori3 { get { return _Kategori3 ?? ""; } set { _Kategori3 = value; } }
        public string Kategori4 { get { return _Kategori4 ?? ""; } set { _Kategori4 = value; } }
        public string Kategori5 { get { return _Kategori5 ?? ""; } set { _Kategori5 = value; } }
        public string Kategori6 { get { return _Kategori6 ?? ""; } set { _Kategori6 = value; } }
    }
}