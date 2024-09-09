using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class MesajlasmaDto
    {
        private Guid _ID;
        private Guid _KullaniciID;
        private Guid _KarsiKullaniciID;
        private String _Mesaj;
        public DateTime _Tarih;
        public MesajlasmaDto() { 
            ID = Guid.NewGuid();
        }
     

        public Guid ID {get { return _ID; }set { _ID = value; }}
        public Guid KullaniciID { get { return _KullaniciID; } set { _KullaniciID = value; } }
        public Guid KarsiKullaniciID { get { return _KullaniciID; } set { _KullaniciID = value; } }
        public string Mesaj { get { return _Mesaj; } set { _Mesaj = value; } }
        public DateTime Tarih { get { return _Tarih == null ? DateTime.Now : _Tarih; } set { _Tarih = value; } }
    }
}