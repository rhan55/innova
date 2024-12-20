using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class KasaViewModel
    {
        private string _ID;
        private string _UyelikID;
        private string _Kod;
        private string _Isim;
        private string _DovizID;
        private string _PersonelID;
        private string _KullaniciID;
        private string _PersonelIsim;
        private string _DovizIsim;
        private string _Personel;
        private string _Doviz;  
   

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string Kod { get { return _Kod ?? ""; } set { _Kod = value; } }
        public string Isim { get { return _Isim ?? ""; } set { _Isim = value; } }
        public string DovizID { get { return _DovizID ?? ""; } set { _DovizID = value; } }
        public string Doviz { get { return _Doviz ?? ""; } set { _Doviz = value; } }
        public string PersonelID { get { return _PersonelID ?? ""; } set { _PersonelID = value; } }
        public string Personel { get { return _Personel ?? ""; } set { _Personel = value; } }
        public string PersonelIsim { get { return _PersonelIsim ?? ""; } set { _PersonelIsim = value; } }
        public string DovizIsim { get { return _DovizIsim ?? ""; } set { _DovizIsim = value; } }
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
        
    }
}
