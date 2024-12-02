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
        private bool _Sil;
        private bool _Duzenle;
        private string _ID;
        private string _StokID;
        private string _UyelikID;
        private bool _Durumu;
        private string _Kod;
        private string _Isim;
        private string _SeriNo;
        private string _Aciklama;
        private string _Aciklama2;
        private string _Barkod;
        private string _Barkod2;
        private string _Barkod3;
        private string _OlcuBirimi;
        private string _GrupKodu1ID;
        private string _GrupKodu2ID;
        private string _GrupKodu3ID;
        private string _GrupKodu4ID;
        private string _GrupKodu5ID;
        private string _GrupKodu6ID;
        private decimal _KdvAlis;
        private decimal _KdvSatis;
        private decimal _Otv;
        private decimal _OtvFiyat;
        private decimal _Oiv;
        private decimal _TevkifatPay;
        private decimal _TevkifatPayda;
        private decimal _IskontoSatis1;
        private decimal _VadeGunu;
        private decimal _MinimumStok;
        private decimal _MaxsimumStok;
        private bool _LimitUyarisi;
        private bool _LimitDisindaIslemiDurdur;
        private bool _EksiBakiyeUyarisi;
        private bool _EksiBakiyedeIslemiDurdur;
        private bool _StokKilitle;
        private string _UreticiFirmaID;
        private string _MarkaID;
        private string _ModelID;
        private string _RenkID;
        private string _BedenID;
        private string _KaliteID;
        private string _KayitYapanKullaniciID;
        private string _AnaStokID;
        private decimal _Bakiye;
        private int _Draw;
        private int _Start;
        private int _Length;
        private string _GrupKodu1Adi;
        private string _GrupKodu2Adi;
      
        public string Dosya { get; set; }
        public string Fiyat { get; set; }
        public string Tarih { get; set; }
        public string BirimFiyat { get; set; }
        public string Mensei { get; set; }
        public byte[] HTMLPrint { get; set; }
        public string Depo { get; set; }

        public bool Sil { get { return _Sil == null ? false : _Sil; } set { _Sil = value; } }
        public bool Duzenle { get { return _Duzenle == null ? false : _Duzenle; } set { _Duzenle = value; } }
        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string StokID { get { return _StokID ?? ""; } set { _StokID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public bool Durumu { get { return _Durumu == null ? false : _Durumu; } set { _Durumu = value; } }
        public string Kod { get { return _Kod ?? ""; } set { _Kod = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
        public string SeriNo { get { return _SeriNo ?? ""; } set { _SeriNo = value; } }
        public string Aciklama { get { return _Aciklama ?? ""; } set { _Aciklama = value; } }
        public string Aciklama2 { get { return _Aciklama2 ?? ""; } set { _Aciklama2 = value; } }
        public string Barkod { get { return _Barkod ?? ""; } set { _Barkod = value; } }
        public string Barkod2 { get { return _Barkod2 ?? ""; } set { _Barkod2 = value; } }
        public string Barkod3 { get { return _Barkod3 ?? ""; } set { _Barkod3 = value; } }
        public string OlcuBirimi { get { return _OlcuBirimi ?? ""; } set { _OlcuBirimi = value; } }
        public string GrupKodu1ID { get { return _GrupKodu1ID ?? ""; } set { _GrupKodu1ID = value; } }
        public string GrupKodu2ID { get { return _GrupKodu2ID ?? ""; } set { _GrupKodu2ID = value; } }
        public string GrupKodu3ID { get { return _GrupKodu3ID ?? ""; } set { _GrupKodu3ID = value; } }
        public string GrupKodu4ID { get { return _GrupKodu4ID ?? ""; } set { _GrupKodu4ID = value; } }
        public string GrupKodu5ID { get { return _GrupKodu5ID ?? ""; } set { _GrupKodu5ID = value; } }
        public string GrupKodu6ID { get { return _GrupKodu6ID ?? ""; } set { _GrupKodu6ID = value; } }
        public decimal KdvAlis { get { return _KdvAlis == null ? 0 : _KdvAlis; } set { _KdvAlis = value; } }
        public decimal KdvSatis { get { return _KdvSatis == null ? 0 : _KdvSatis; } set { _KdvSatis = value; } }
        public decimal Otv { get { return _Otv == null ? 0 : _Otv; } set { _Otv = value; } }
        public decimal OtvFiyat { get { return _OtvFiyat == null ? 0: _OtvFiyat; } set { _OtvFiyat = value; } }
        public decimal Oiv { get { return _Otv == null ? 0 : _Oiv; } set { _Oiv = value; } }
        public decimal TevkifatPay { get { return _TevkifatPay == null ? 0 : _TevkifatPay; } set { _TevkifatPay = value; } }
        public decimal TevkifatPayda { get { return _TevkifatPayda == null ? 0 : _TevkifatPayda; } set { _TevkifatPayda = value; } }
        public decimal IskontoSatis1 { get { return _IskontoSatis1 == null ? 0 : _IskontoSatis1; } set { _IskontoSatis1 = value; } }
        public decimal VadeGunu { get { return _VadeGunu == null ? 0 : _VadeGunu; } set { _VadeGunu = value; } }
        public decimal MinimumStok { get { return _MinimumStok == null ? 0 : _MinimumStok; } set { _MinimumStok = value; } }
        public decimal MaxsimumStok { get { return _MaxsimumStok == null ? 0 : _MaxsimumStok; } set { _MaxsimumStok = value; } }
        public bool LimitUyarisi { get { return _LimitUyarisi == null ? false : _LimitUyarisi; } set { _LimitUyarisi = value; } }
        public bool LimitDisindaIslemiDurdur { get { return _LimitDisindaIslemiDurdur == null ? false : _LimitDisindaIslemiDurdur; } set { _LimitDisindaIslemiDurdur = value; } }
        public bool EksiBakiyeUyarisi { get { return _EksiBakiyeUyarisi == null ? false : _EksiBakiyeUyarisi; } set { _EksiBakiyeUyarisi = value; } }
        public bool EksiBakiyedeIslemiDurdur { get { return _EksiBakiyedeIslemiDurdur == null ? false : _EksiBakiyedeIslemiDurdur; } set { _EksiBakiyedeIslemiDurdur = value; } }
        public bool StokKilitle { get { return _StokKilitle == null ? false : _StokKilitle; } set { _StokKilitle = value; } }
        public string UreticiFirmaID { get { return _UreticiFirmaID ?? ""; } set { _UreticiFirmaID = value; } }
        public string MarkaID { get { return _MarkaID ?? ""; } set { _MarkaID = value; } }
        public string ModelID { get { return _ModelID ?? ""; } set { _ModelID = value; } }
        public string RenkID { get { return _RenkID ?? ""; } set { _RenkID = value; } }
        public string BedenID { get { return _BedenID ?? ""; } set { _BedenID = value; } }
        public string KaliteID { get { return _KaliteID ?? ""; } set { _KaliteID = value; } }
        public string KayitYapanKullaniciID { get { return _KayitYapanKullaniciID ?? ""; } set { _KayitYapanKullaniciID = value; } }
        public string AnaStokID { get { return _AnaStokID ?? ""; } set { _AnaStokID = value; } }
        public decimal Bakiye { get { return _Bakiye == null ? 0 : _Bakiye; } set { _Bakiye = value; } }
        public int Draw { get { return _Draw == null ? 0 : _Draw; } set { _Draw = value; } }
        public int Start { get { return _Start == null ? 0 : _Start; } set { _Start = value; } }
        public int Length { get { return _Length == null ? 0 : _Length; } set { _Length = value; } }
        public string GrupKodu1Adi { get { return _GrupKodu1Adi ?? ""; } set { _GrupKodu1Adi = value; } }
        public string GrupKodu2Adi { get { return _GrupKodu2Adi ?? ""; } set { _GrupKodu2Adi = value; } }

    }
} 


