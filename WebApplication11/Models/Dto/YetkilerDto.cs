using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class YetkilerDto
    {
        private string _UyelikID;
        private string _KullaniciID;
        private string _MenuID;
        private bool _Gor;
        private bool _Duzenle;
        private bool _Sil;

        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
        public string MenuID { get { return _MenuID ?? ""; } set { _MenuID = value; } }
        public bool Gor { get { return _Gor == null ? false : _Gor; } set { _Gor = value; } }      
        public bool Duzenle { get { return _Duzenle == null ? false : _Duzenle; } set { _Duzenle = value; } }
        public bool Sil { get { return _Sil == null ? false : _Sil; } set { _Sil = value; } }
    }
}

