using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class PersonelListeViewModel
    {
        public List<PersonelDto> Personeller { get; set; }
        public bool Duzenle { get; set; }
        public bool Sil { get; set; }
    }
}