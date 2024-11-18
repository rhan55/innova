using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class EFaturaDto
    {
        public string CariID { get; set; }
        public string EFaturaPKAdresi { get; set; } = string.Empty;
        public DateTime EFaturaGecisTarihi { get; set; }
        public string Unvan { get; set; } = string.Empty;
        public string VergiNumarasi { get; set; } = string.Empty;
        public bool EFaturaAktiflik { get; set; } 
        public bool EFatura { get; set; }
        public string Tip { get; set; } = string.Empty;
        public string EIrsaliyePKAdresi { get; set; } = string.Empty;
        public bool EIrsaliyeAktiflik { get; set; }
        public bool EIrsaliye { get; set; }
        public string EFaturaSenaryo { get; set; } = string.Empty;

    }
}