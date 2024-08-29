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
    private DateTime _Tarih;
    private string _Durumu;
    private string _Baslik;
    private string _Aciklama;
     

    public string ID { get { return _ID ?? ""; } set { _ID = value; } }
    public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
    public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
    public DateTime Tarih { get { return _Tarih == null ? DateTime.Now : _Tarih; } set { _Tarih = value; } }
    public string Aciklama { get { return _Aciklama ?? ""; } set { _Aciklama = value; } }
    public string Durumu { get { return _Durumu ?? ""; } set { _Durumu = value; } }
    public string Baslik { get { return _Baslik ?? ""; } set { _Baslik = value; } }
 
}

}

