using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class BankaTanimlamalariDto
    {
        private string _ID;
        private string _UyelikID;
        private string _BankaID;
        private string _Kod;
        private string _Isim;
        private string _HesapNo;
        private string _Iban;
        private string _KullaniciID;
        private string _Banka;
        private string _KayitTarihi;
        private string _KayitYapanKullanici;
        private string _DuzenlemeTarihi;
        private string _DuzenlemeYapanKullanici;

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string BankaID { get { return _BankaID ?? ""; } set { _BankaID = value; } }
        public string Kod { get { return _Kod ?? ""; } set { _Kod = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
        public string HesapNo { get { return _HesapNo ?? ""; } set { _HesapNo = value; } }
        public string Iban { get { return _Iban ?? ""; } set { _Iban = value; } }       
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
        public string Banka { get { return _Banka ?? ""; } set { _Banka = value; } }

    }
}

