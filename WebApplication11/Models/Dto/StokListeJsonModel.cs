using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class StokListeJsonModel
    {
        public List<StokDto> StokListesi { get; set; } = new List<StokDto>();
        public bool Sil { get; set; }
        public bool Duzenle { get; set; }

        public StokListeJsonModel()
        {
            StokListesi = new List<StokDto>();
        }
    }
}