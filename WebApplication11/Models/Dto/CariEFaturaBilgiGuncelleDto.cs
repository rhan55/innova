using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class CariEFaturaBilgiGuncelleDto
    {
     
        private string _UyelikID;
        private string _CariID;
        private string _EFaturaKayitTarihi;
        private string _EFaturaPKAdresi;
        private string _EIrsaliyePKAdresi;
        private bool _EFatura;
        private bool _EIrsaliye;
        private string _EFaturaSenaryo;
        private bool _EFaturaAktiflik;
        private bool _EIrsaliyeAktiflik;



        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string CariID { get { return _CariID ?? ""; } set { _CariID = value; } }
        public string EFaturaKayitTarihi { get { return _EFaturaKayitTarihi ?? ""; } set { _EFaturaKayitTarihi = value; } }
        public string EFaturaPKAdresi { get { return _EFaturaPKAdresi ?? ""; } set { _EFaturaPKAdresi = value; } }
        public string EIrsaliyePKAdresi { get { return _EIrsaliyePKAdresi ?? ""; } set { _EIrsaliyePKAdresi = value; } }      
        public bool EFatura { get { return _EFatura == null ? false : _EFatura; } set { _EFatura = value; } }
        public bool EIrsaliye { get { return _EIrsaliye == null ? false : _EIrsaliye; } set { _EIrsaliye = value; } }    
        public string EFaturaSenaryo { get { return _EFaturaSenaryo ?? ""; } set { _EFaturaSenaryo = value; } }
        public bool EFaturaAktiflik { get { return _EFaturaAktiflik == null ? false : _EFaturaAktiflik; } set { _EFaturaAktiflik = value; } }
        public bool EIrsaliyeAktiflik { get { return _EIrsaliyeAktiflik == null ? false : _EIrsaliyeAktiflik; } set { _EIrsaliyeAktiflik = value; } }


 
    }
}