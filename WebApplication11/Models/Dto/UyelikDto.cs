using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class UyelikDto

    {
        private string _ID;
        private string _Isim;
        private string _Unvan;
        private string _VergiNumarasi;
        private string _VergiDairesi;
        private string _Adres;
        private string _Il;
        private string _Ilce;
        private string _EMail;
        private string _Ad;
        private string _Soyad;
        private string _Parola;
        private string _Iletisim;
        private string _Kullanici;
        private string _UyelikBaslangicTarihi;
        private string _UyelikBitisTarihi;
        private string _ApiUrl;
        private string _AcilisSayfasi;

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }

        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }

        public string Unvan { get { return _Unvan ?? ""; } set { _Unvan = value; } }

        [StringLength(maximumLength: 11, MinimumLength = 10, ErrorMessage = "Vergi Numarası 10 ya da 11 haneli olmalıdır.")]
        public string VergiNumarasi { get { return _VergiNumarasi ?? ""; } set { _VergiNumarasi = value; } }

        public string VergiDairesi { get { return _VergiDairesi ?? ""; } set { _VergiDairesi = value; } }

        public string Adres { get { return _Adres ?? ""; } set { _Adres = value; } }

        public string Il { get { return _Il ?? ""; } set { _Il = value; } }

        public string Ilce { get { return _Ilce ?? ""; } set { _Ilce = value; } }

        public string EMail { get { return _EMail ?? ""; } set { _EMail = value; } }

        public string Ad { get { return _Ad ?? ""; } set { _Ad = value; } }

        public string Soyad { get { return _Soyad ?? ""; } set { _Soyad = value; } }

        public string Parola { get { return _Parola ?? ""; } set { _Parola = value; } }

        [StringLength(maximumLength: 11, MinimumLength = 10, ErrorMessage = "Telefon Numaranız 10 ya da 11 haneli olmalıdır.")]
        [Phone(ErrorMessage = "Telefon numarasını doğru girdiğinizden emin olunuz!")]
        public string Iletisim { get { return _Iletisim ?? ""; } set { _Iletisim = value; } }

        public string Kullanici { get { return _Kullanici ?? ""; } set { _Kullanici = value; } }

        public string UyelikBaslangicTarihi { get { return _UyelikBaslangicTarihi ?? ""; } set { _UyelikBaslangicTarihi = value; } }

        public string UyelikBitisTarihi { get { return _UyelikBitisTarihi ?? ""; } set { _UyelikBitisTarihi = value; } }

        public string ApiUrl { get { return _ApiUrl ?? ""; } set { _ApiUrl = value; } }
        public string AcilisSayfasi { get { return _AcilisSayfasi ?? ""; } set { _AcilisSayfasi = value; } }

    }
}

