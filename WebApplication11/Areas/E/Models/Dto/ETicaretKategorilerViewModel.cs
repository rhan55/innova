using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Areas.E.Models.Dto
{
    public class ETicaretKategorilerViewModel
    {
        public List<ETicaretKategorilerDto> Kategoriler { get; set; }
        public string ErrorMessage { get; set; }
    }
}