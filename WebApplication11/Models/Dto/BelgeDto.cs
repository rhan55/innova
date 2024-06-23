using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class BelgeDto
    {

        private string _ID;
        private BelgeTipi _BelgeTipi;
        private string _BelgeNo;
        private DateTime _Tarih;
        private string _CariID;
        private string _CariAdi;
        private string _Aciklama;
        private List<BelgeKalemDto> _Kalemler;
        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public BelgeTipi BelgeTipi { get { return _BelgeTipi == null ? BelgeTipi.Genel : _BelgeTipi; } set { _BelgeTipi = value; } }
        public string BelgeNo { get { return _BelgeNo ?? ""; } set { _BelgeNo = value; } }
        public DateTime Tarih { get { return _Tarih == null ? DateTime.Now : _Tarih; } set { _Tarih = value; } }
        public string CariID { get { return _CariID ?? ""; } set { _CariID = value; } }
        public string CariAdi { get { return _CariAdi ?? ""; } set { _CariAdi = value; } }
        public string Aciklama { get { return _Aciklama ?? ""; } set { _Aciklama = value; } }
        public List<BelgeKalemDto> Kalemler { get { return _Kalemler == null ? new List<BelgeKalemDto>() : _Kalemler; } set { _Kalemler = value; } }
    }

    public enum BelgeTipi
    {
        Genel,
        SatisSiparisi,
        SatinalmaSiparisi,
        AlisIrsaliyesi,
        SatisIrsaliyesi,
        SatisFaturasi,
        SatinalmaFaturasi,
        DepoTransferi
    }
}