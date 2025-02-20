using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YKEFaturaEntegrasyon.EFaturaEDM;

namespace YKEFaturaEntegrasyon.Dto
{
    public class InvoiceDto
    {
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
}
