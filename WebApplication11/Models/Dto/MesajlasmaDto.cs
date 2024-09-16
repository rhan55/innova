using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class MesajlasmaDto
    {
        private Guid _ID;
        private string _KullaniciID;
        private string _KarsiKullaniciID;
        private String _Mesaj;
        private string _Tarih;
        public MesajlasmaDto() { 
            ID = Guid.NewGuid();
        }


        public Guid ID {get { return _ID; }set { _ID = value; }}
        public string KullaniciID { get { return _KullaniciID; } set { _KullaniciID = value; } }
        public string KarsiKullaniciID { get { return _KarsiKullaniciID; } set { _KarsiKullaniciID = value; } }
        public string Mesaj { get { return _Mesaj; } set { _Mesaj = value; } }
        public string Tarih { get { return _Tarih; } set { _Tarih = value; } }
     
    }
}