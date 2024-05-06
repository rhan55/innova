using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication11.Models.Dto
{
    public class UyelikDto
    {
        public string ID { get; set; }
        public string Isim { get; set; }
        public string Unvan { get; set; }
        public string VergiNumarasi { get; set; }
        public string VergiDairesi { get; set; }
        public string Adres { get; set; }
        public string EMail { get; set; }
        public string Iletisim { get; set; }
        public string Kullanici { get; set; }
        public string UyelikBaslangicTarihi { get; set; }
        public string UyelikBitisTarihi { get; set; }
        public string ApiUrl { get; set; }
    }
}

