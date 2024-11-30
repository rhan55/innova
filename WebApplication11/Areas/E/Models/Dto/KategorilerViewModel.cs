using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Areas.E.Models.Dto
{
    public class KategorilerViewModel
    {
        public List<KategorilerDto> Kategoriler { get; set; }
        public string ErrorMessage { get; set; }
    }
}