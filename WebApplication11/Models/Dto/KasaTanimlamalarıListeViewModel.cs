using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class BankaTanimlamalariDtoListeViewModel
    {
        public List<KullaniciListeViewModel> Kasalar1 { get; set; }
        public List<KasaViewModel> Kasalar { get; set; }
        public bool Duzenle { get; set; }
        public bool Sil { get; set; }
    }
}
