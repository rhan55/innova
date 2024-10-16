using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class FullcalendarDto
    {
        public string id { get; set; }
        public string title { get; set; }
        public long start { get; set; }
        public long end { get; set; }
        public string description { get; set; }
        public string customStatus { get; set; }
    }
}