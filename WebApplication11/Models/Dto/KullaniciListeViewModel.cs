using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class KullaniciListeViewModel
    {
        public List<KullaniciEkleDto> KullaniciListesi { get; set; }

        public DataTable Kullanicilar { get; set; }
        public bool Sil { get; set; }
        public bool Duzenle { get; set; }
    }
}