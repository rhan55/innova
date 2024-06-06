using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class CariDto
    {
        private string _ID; 
        private string _UyelikID;
        private bool _Aktif;
        private string _KayitTarihi;
        private string _Kod;
        private string _Isim;
        private string _Unvan;
        private string _Adres;
        private string _Ilce;
        private string _Il;
        private string _Ulke;
        private string _Bolge;
        private string _TCKimlikNo;
        private string _VergiDairesi;
        private string _VergiNumarasi;
        private string _PostaKodu;
        private bool _Alici;
        private bool _Satici;
        private bool _Personel;
        private string _Telefon1;
        private string _Telefon2;
        private string _EMail;
        private string _Faks;
        private string _CepTelefonu;
        private string _WebSite;
        private string _GrupKodu1ID;
        private string _GrupKodu2ID;
        private string _GrupKodu3ID;
        private string _GrupKodu4ID;
        private string _GrupKodu5ID;
        private string _GrupKodu6ID;
        private string _MuhasebeKodu;
        private bool _Kilitli;
        private string _KilitAciklamasi;
        private string _DovizID;
        private int _VadeGunu;
        private decimal _Iskonto1;
        private decimal _ListeFiyat;
        private string _Aciklama1;
        private string _Aciklama2;
        private string _Aciklama3;
        private string _Aciklama4;
        private string _Aciklama5;
        private string _Aciklama6;
        private bool _LimitAsimindaUyar;
        private bool _LimitAsimindaDurdur;
        private bool _CekSenetRiski;
        private decimal _Limit;
        private bool _ServisPersoneli;
        private string _KullaniciAdi;
        private string _Parola;
        private string _RiskAciklama;
        private string _PlasiyerID;
        private string _AnaCariID;
        private string _TeslimCariID;
        private string _KullaniciID;



        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public bool Aktif { get { return _Aktif == null ? false : _Aktif; } set { _Aktif = value; } }
        public string KayitTarihi { get { return _KayitTarihi ?? ""; } set { _KayitTarihi = value; } }
        public string Kod { get { return _Kod ?? ""; } set { _Kod = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
        public string Unvan { get { return _Unvan ?? ""; } set { _Unvan = value; } }
        public string Adres { get { return _Adres ?? ""; } set { _Adres = value; } }
        public string Ilce { get { return _Ilce ?? ""; } set { _Ilce = value; } }
        public string Il { get { return _Il ?? ""; } set { _Il = value; } }
        public string Ulke { get { return _Ulke ?? ""; } set { _Ulke = value; } }
        public string Bolge { get { return _Bolge ?? ""; } set { _Bolge = value; } }
        public string TCKimlikNo { get { return _TCKimlikNo ?? ""; } set { _TCKimlikNo = value; } }
        public string VergiDairesi { get { return _VergiDairesi ?? ""; } set { _VergiDairesi = value; } }
        public string VergiNumarasi { get { return _VergiNumarasi ?? ""; } set { _VergiNumarasi = value; } }
        public string PostaKodu { get { return _PostaKodu ?? ""; } set { _PostaKodu = value; } }
        public bool Alici { get { return _Alici == null ? false : _Alici; } set { _Alici = value; } }
        public bool Satici { get { return _Satici == null ? false : _Satici; } set { _Satici = value; } }
        public bool Personel { get { return _Personel == null ? false : _Personel; } set { _Personel = value; } }
        public string Telefon1 { get { return _Telefon1 ?? ""; } set { _Telefon1 = value; } }
        public string Telefon2 { get { return _Telefon2 ?? ""; } set { _Telefon2 = value; } }
        public string EMail { get { return _EMail ?? ""; } set { _EMail = value; } }
        public string Faks { get { return _Faks ?? ""; } set { _Faks = value; } }
        public string CepTelefonu { get { return _CepTelefonu ?? ""; } set { _CepTelefonu = value; } }
        public string WebSite { get { return _WebSite ?? ""; } set { _WebSite = value; } }
        public string GrupKodu1ID { get { return _GrupKodu1ID ?? ""; } set { _GrupKodu1ID = value; } }
        public string GrupKodu2ID { get { return _GrupKodu2ID ?? ""; } set { _GrupKodu2ID = value; } }
        public string GrupKodu3ID { get { return _GrupKodu3ID ?? ""; } set { _GrupKodu3ID = value; } }
        public string GrupKodu4ID { get { return _GrupKodu4ID ?? ""; } set { _GrupKodu4ID = value; } }
        public string GrupKodu5ID { get { return _GrupKodu5ID ?? ""; } set { _GrupKodu5ID = value; } }
        public string GrupKodu6ID { get { return _GrupKodu6ID ?? ""; } set { _GrupKodu6ID = value; } }
        public string MuhasebeKodu { get { return _MuhasebeKodu ?? ""; } set { _MuhasebeKodu = value; } }
        public bool Kilitli { get { return _Kilitli == null ? false : _Kilitli; } set { _Kilitli = value; } }
        public string KilitAciklamasi { get { return _KilitAciklamasi ?? ""; } set { _KilitAciklamasi = value; } }
        public string DovizID { get { return _DovizID ?? ""; } set { _DovizID = value; } }
        public int VadeGunu { get { return _VadeGunu == null ? 0 : _VadeGunu; } set { _VadeGunu = value; } }
        public decimal Iskonto1 { get { return _Iskonto1 == null ? 0 : _Iskonto1; } set { _Iskonto1 = value; } }
        public decimal ListeFiyat { get { return _ListeFiyat == null ? 1 : _ListeFiyat; } set { _ListeFiyat = value; } }
        public string Aciklama1 { get { return _Aciklama1 ?? ""; } set { _Aciklama1 = value; } }
        public string Aciklama2 { get { return _Aciklama2 ?? ""; } set { _Aciklama2 = value; } }
        public string Aciklama3 { get { return _Aciklama3 ?? ""; } set { _Aciklama3 = value; } }
        public string Aciklama4 { get { return _Aciklama4 ?? ""; } set { _Aciklama4 = value; } }
        public string Aciklama5 { get { return _Aciklama5 ?? ""; } set { _Aciklama5 = value; } }
        public string Aciklama6 { get { return _Aciklama6 ?? ""; } set { _Aciklama6 = value; } }
        public bool LimitAsimindaUyar { get { return _LimitAsimindaUyar == null ? false : _LimitAsimindaUyar; } set { _LimitAsimindaUyar = value; } }
        public bool LimitAsimindaDurdur { get { return _LimitAsimindaDurdur == null ? false : _LimitAsimindaDurdur; } set { _LimitAsimindaDurdur = value; } }
        public bool CekSenetRiski { get { return _CekSenetRiski == null ? false : _CekSenetRiski; } set { _CekSenetRiski = value; } }
        public decimal Limit { get { return _Limit == null ? 0 : _Limit; } set { _Limit = value; } }
        public bool ServisPersoneli { get { return _ServisPersoneli == null ? false : _ServisPersoneli; } set { _ServisPersoneli = value; } }
        public string KullaniciAdi { get { return _KullaniciAdi ?? ""; } set { _KullaniciAdi = value; } }
        public string Parola { get { return _Parola ?? ""; } set { _Parola = value; } }
        public string RiskAciklama { get { return _RiskAciklama ?? ""; } set { _RiskAciklama = value; } }
        public string PlasiyerID { get { return _PlasiyerID ?? ""; } set { _PlasiyerID = value; } }
        public string AnaCariID { get { return _AnaCariID ?? ""; } set { _AnaCariID = value; } }
        public string TeslimCariID { get { return _TeslimCariID ?? ""; } set { _TeslimCariID = value; } }
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }

    }
}

