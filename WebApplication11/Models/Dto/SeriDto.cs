using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
   
    public class SeriDto
    {

        public string ID { get; set; } = string.Empty;
        public string Kod { get; set; } = string.Empty;
        public string Deger { get; set; } = string.Empty;
        public bool Aktif { get; set; } = false;

    }
}