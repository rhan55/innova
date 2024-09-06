using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class AnasayfaTakvimKaydetDto
    {

    private string _ID;
    private string _UyelikID;
    private string _KullaniciID;
    private string _KayitYapanKullanici;
    private string _DuzenlemeYapanKullanici;
    private DateTime _Tarih;
    private DateTime _BaslangicTarihi;
    private DateTime _BitisTarihi;
    private string _Durumu;
    private string _Baslik;
    private string _Aciklama;
     

    public string ID { get { return _ID ?? ""; } set { _ID = value; } }
    public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
    public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
    public string KayitYapanKullanici { get { return _KayitYapanKullanici ?? ""; } set { _KayitYapanKullanici = value; } }
    public string DuzenlemeYapanKullanici { get { return _DuzenlemeYapanKullanici ?? ""; } set { _DuzenlemeYapanKullanici = value; } }
    public DateTime Tarih { get { return _Tarih == null ? DateTime.Now : _Tarih; } set { _Tarih = value; } }
    public string Aciklama { get { return _Aciklama ?? ""; } set { _Aciklama = value; } }
    public DateTime BaslangicTarihi { get { return _Tarih == null ? DateTime.Now : _Tarih; } set { _Tarih = value; } }
    public DateTime BitisTarihi { get { return _BitisTarihi == null ? DateTime.Now : _BitisTarihi; } set { _BitisTarihi = value; } }
    public string Durumu { get { return _Durumu ?? ""; } set { _Durumu = value; } }
    public string Baslik { get { return _Baslik ?? ""; } set { _Baslik = value; } }


    }

}

