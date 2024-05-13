using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class SonAktivitelerDto
    {

        private string _Tarih;
        private string _Modul;
        private string _Aciklama1;
        private string _Aciklama2;
        private string _Kullanici;

        public string Tarih { get { return _Tarih ?? ""; } set { _Tarih = value; } }
        public string Modul { get { return _Modul ?? ""; } set { _Modul = value; } }
        public string Aciklama1 { get { return _Aciklama1 ?? ""; } set { _Aciklama1 = value; } }
        public string Aciklama2 { get { return _Aciklama2 ?? ""; } set { _Aciklama2 = value; } }
        public string Kullanici { get { return _Kullanici ?? ""; } set { _Kullanici = value; } }

    }
}