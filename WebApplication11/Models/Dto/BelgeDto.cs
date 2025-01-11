using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YKEFaturaEntegrasyon.EFaturaEDM;

namespace YKPortal.Models.Dto
{
    public class BelgeDto
    {

        private string _ID;
        private BelgeTipi _BelgeTipi;
        private string _BelgeNo;
        private DateTime _Tarih;
        private DateTime _BaslangicTarihi;
        private DateTime _BitisTarihi;
        private string _ProjeID;
        private string _CariID;
        private string _CariAdi;
        private string _CariAdres;
        private string _Durumu;
        private string _Aciklama;
        private string _SatisPersonelID;
        private string _DepoGirisID;
        private string _DepoCikisID;
        


        private List<BelgeKalemDto> _Kalemler;
        public string ID { get { return _ID ?? ""; } set { _ID = value; } }
        public BelgeTipi BelgeTipi { get { return _BelgeTipi == null ? BelgeTipi.Genel : _BelgeTipi; } set { _BelgeTipi = value; } }
        public string BelgeNo { get { return _BelgeNo ?? ""; } set { _BelgeNo = value; } }
        public DateTime Tarih { get { return _Tarih == null ? DateTime.Now : _Tarih; } set { _Tarih = value; } }
        public DateTime BaslangicTarihi { get { return _BaslangicTarihi == null ? DateTime.Now : _BaslangicTarihi; } set { _BaslangicTarihi = value; } }
        public DateTime BitisTarihi { get { return _BitisTarihi == null ? DateTime.Now : _BitisTarihi; } set { _BitisTarihi = value; } }
        public string ProjeID { get { return _ProjeID ?? ""; } set { _ProjeID = value; } }
        public string CariID { get { return _CariID ?? ""; } set { _CariID = value; } }
        public string CariAdi { get { return _CariAdi ?? ""; } set { _CariAdi = value; } }
        public string CariAdres { get { return _CariAdres ?? ""; } set { _CariAdres = value; } }
        public string Durumu { get { return _Durumu ?? ""; } set { _Durumu = value; } }
        public string Aciklama { get { return _Aciklama ?? ""; } set { _Aciklama = value; } }
        public string SatisPersonelID { get { return _SatisPersonelID ?? ""; } set { _SatisPersonelID = value; } }
        public string DepoGirisID { get { return _DepoGirisID ?? ""; } set { _DepoGirisID = value; } }
        public string DepoCikisID { get { return _DepoCikisID ?? ""; } set { _DepoCikisID = value; } }
        public List<BelgeKalemDto> Kalemler { get { return _Kalemler == null ? new List<BelgeKalemDto>() : _Kalemler; } set { _Kalemler = value; } }

    }
    public class InvoiceDto
    {
        public string Customer { get; set; } // Fatura alıcı ismi
        public string Supplier { get; set; } // Tedarikçi
        public decimal Amount { get; set; } // Fatura tutarı
        public DateTime IssueDate { get; set; } // Fatura tarihi
        public Guid ID { get; set; } // Fatura UUID
        public int CariID { get; set; } // Cari ID
        public string Unvan { get; set; } // Firma unvanı
        public DateTime BaslangicTarihi { get; set; } // Fatura başlangıç tarihi
        public INVOICEHEADER HEADER { get; set; }
        public string UUID { get; set; }
        public base64Binary CONTENT { get; set; }
        public string SENDER { get; set; }
        public string RECEIVER { get; set; }
        public string SUPPLIER { get; set; }
        public string CUSTOMER { get; set; }
        public DateTime ISSUE_DATE { get; set; }
        public bool ISSUE_DATESpecified { get; set; }
        public byte[] Value { get; set; }
    }
    public enum BelgeTipi
    {
        Genel,
        SatisTalebi,
        SatinalmaTalebi,
        AlisIrsaliyesi,
        SatisIrsaliyesi,
        SatisFaturasi,
        SatinalmaTeklifi,
        DepoTransferi,
        SatinalmaSiparisi
    }
}