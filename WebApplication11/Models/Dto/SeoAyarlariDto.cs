using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class SeoAyarlariDto
    {

        private string _SeoDescription;
        private string _SeoKeywords;
        private string _FirmaAdi;
        private bool _SSLYonlendir;
        private bool _AnaSayfadaAcilisSayfasiKontrolu;
        private bool _IlkUyelikdeKullaniciyiOnayliYap;
        private bool _YeniUyelikKaydi;
        private bool _SifremiUnuttum;



        public string SeoDescription { get { return _SeoDescription ?? ""; } set { _SeoDescription = value; } }
        public string SeoKeywords { get { return _SeoKeywords ?? ""; } set { _SeoKeywords = value; } }
        public string FirmaAdi { get { return _FirmaAdi ?? ""; } set { _FirmaAdi = value; } }
        public bool SSLYonlendir { get { return _SSLYonlendir == null ? false : _SSLYonlendir; } set { _SSLYonlendir = value; } }
        public bool AnaSayfadaAcilisSayfasiKontrolu { get { return _AnaSayfadaAcilisSayfasiKontrolu == null ? false : _AnaSayfadaAcilisSayfasiKontrolu; } set { _AnaSayfadaAcilisSayfasiKontrolu = value; } }
        public bool IlkUyelikdeKullaniciyiOnayliYap { get { return _IlkUyelikdeKullaniciyiOnayliYap == null ? false : _IlkUyelikdeKullaniciyiOnayliYap; } set { _IlkUyelikdeKullaniciyiOnayliYap = value; } }
        public bool YeniUyelikKaydi { get { return _YeniUyelikKaydi == null ? false : _YeniUyelikKaydi; } set { _YeniUyelikKaydi = value; } }
        public bool SifremiUnuttum { get { return _SifremiUnuttum == null ? false : _SifremiUnuttum; } set { _SifremiUnuttum = value; } }
    }
}