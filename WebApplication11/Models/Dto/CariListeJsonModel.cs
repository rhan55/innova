using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class CariListeJsonModel
    {
        public List<CariDto> CariListesi { get; set; } = new List<CariDto>();
        public bool Sil { get; set; }
        public bool Duzenle { get; set; }
    }
}