using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using YKPortal.Models.Dto;

namespace YKPortal.Models.ComponentDto
{
    public class BelgeEkleComponent
    {
        public bool BelgeNoVarMi { get; set; } = true;
        public bool AciklamaVarMi { get; set; } = true;
        public bool CariVarMi { get; set; } = true;
        public bool CikisDepoVarMi { get; set; } = true;
        public bool SatisPersoneliVarMi { get; set; } = true;
        public bool DurumuVarMi { get; set; } = true;
        public string Tip { get; set; }
        public string Baslik { get; set; }
        public BelgeDto BelgeDto { get; set; }
        public BelgeEkleKalemComponent KalemBilgileri { get; set; }
        public DataTable Depolar { get; set; }
        public List<SatisPersonelleriDto> SatisPersonelleri { get; set; }

    }


    public class BelgeEkleKalemComponent
    {
        public bool UrunVarMi { get; set; } = true;
        public bool SeriVarMi { get; set; } = true;
        public bool BirimVarMi { get; set; } = true;
        public bool KdvVarMi { get; set; } = true;
        public bool MiktarVarMi { get; set; } = true;
        public bool FiyatVarMi { get; set; } = true;
        public bool IskontoVarMi { get; set; } = true;
        public bool TutarVarMi { get; set; } = true;
    }
}