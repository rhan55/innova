using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace YKPortal.Models.Dto
{
    public class StokFiyatDto
    {

        private string _ID;
        private string _UyelikID;
        private string _StokID;
        private string _KullaniciID;
        private string _CariID;
        private string _FiyatGrubu;
        private string _Tip;       
        private decimal _Fiyat;
        private DateTime _BaslangicTarihi;
        private DateTime _BitisTarihi;

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string StokID { get { return _StokID ?? ""; } set { _StokID = value; } }
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
        public string CariID { get { return _CariID ?? ""; } set { _CariID = value; } }
        public string FiyatGrubu { get { return _FiyatGrubu ?? ""; } set { _FiyatGrubu = value; } }
        public string Tip { get { return _Tip ?? ""; } set { _Tip = value; } }
        public decimal Fiyat { get { return _Fiyat == null ? 0 : _Fiyat; } set { _Fiyat = value; } }
        public DateTime BaslangicTarihi { get { return _BaslangicTarihi == null ? DateTime.Today : _BaslangicTarihi; } set { _BaslangicTarihi = value; } }
        public DateTime BitisTarihi { get { return _BitisTarihi == null ? DateTime.Today : _BitisTarihi; } set { _BitisTarihi = value; } }

    }
}



 