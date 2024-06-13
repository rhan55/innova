using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class MailAyarlariDto
    {
        private string _Host;
        private string _Port;
        private string _KullaniciAdi;
        private string _Parola;
        private string _Isim;
        private bool _SSL;
        public string Host { get { return _Host ?? ""; } set { _Host = value; } }
        public string Port { get { return _Port ?? ""; } set { _Port = value; } }
        public string KullaniciAdi { get { return _KullaniciAdi ?? ""; } set { _KullaniciAdi = value; } }
        public string Parola { get { return _Parola ?? ""; } set { _Parola = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
        public bool SSL { get { return _SSL == null ? false : _SSL; } set { _SSL = value; } }

    }
}