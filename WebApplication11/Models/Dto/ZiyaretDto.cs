using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class ZiyaretDto
    {

        private string _ID;
        private string _UyelikID;
        private string _CariID;
        private string _Tarih;
        private string _ZiyaretTipi;
        private string _Aciklama;
        private string _TamamlamaAciklamasi;
        private string _TamamlamaTarihi;
        private string _TamamlayanKullaniciID;
        private string _KullaniciID;
        private string _CariIsim;
        private string _KaydiAcanIsim;
        private string _TamamlayanIsim;

        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public string UyelikID { get { return _UyelikID ?? ""; } set { _UyelikID = value; } }
        public string CariID { get { return _CariID ?? ""; } set { _CariID = value; } }
        public string Tarih { get { return _Tarih ?? string.Empty; } set { _Tarih = value; } }
        public string ZiyaretTipi { get { return _ZiyaretTipi ?? ""; } set { _ZiyaretTipi = value; } }
        public string Aciklama { get { return _Aciklama ?? ""; } set { _Aciklama = value; } }
        public string TamamlamaAciklamasi { get { return _TamamlamaAciklamasi == null ? string.Empty : _TamamlamaAciklamasi; } set { _TamamlamaAciklamasi = value; } }
        public string TamamlamaTarihi { get { return _TamamlamaTarihi ?? string.Empty; } set { _TamamlamaTarihi = value; } }
        public string TamamlayanKullaniciID { get { return _TamamlayanKullaniciID ?? ""; } set { _TamamlayanKullaniciID = value; } }
        public string KullaniciID { get { return _KullaniciID ?? ""; } set { _KullaniciID = value; } }
        public string CariIsim { get { return _CariIsim ?? ""; } set { _CariIsim = value; } }
        public string KaydiAcanIsim { get { return _KaydiAcanIsim ?? ""; } set { _KaydiAcanIsim = value; } }
        public string TamamlayanIsim { get { return _TamamlayanIsim ?? ""; } set { _TamamlayanIsim = value; } }

    }
}
