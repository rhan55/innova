using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class SmsAyarlariDto
    {
        private string _SmsKullaniciAdi;
        private string _SmsParola;
        private string _SmsIsim;



        public string SmsKullaniciAdi { get { return _SmsKullaniciAdi ?? ""; } set { _SmsKullaniciAdi = value; } }
        public string SmsParola { get { return _SmsParola ?? ""; } set { _SmsParola = value; } }
        public string SmsIsim { get { return _SmsIsim ?? ""; } set { _SmsIsim = value; } }
    }
}