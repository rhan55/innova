
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class GrupkoduListeViewModel
    {
        public DataTable GrupKodlari { get; set; }
        public bool Sil { get; set; }
        public bool Duzenle { get; set; }
    }
}