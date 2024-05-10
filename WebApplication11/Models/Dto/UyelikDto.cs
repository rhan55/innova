using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class UyelikDto
    {
        public string ID { get; set; }
     
        public string Isim { get; set; }
   
        public string Unvan { get; set; }
        [StringLength(maximumLength:11, MinimumLength = 10, ErrorMessage = "Vergi Numarası 10 ya da 11 haneli olmalıdır.")]
        public string VergiNumarasi { get; set; }
       
        public string VergiDairesi { get; set; }
       
        public string Adres { get; set; }
     
        public string Il { get; set; }
        
        public string Ilce { get; set; }
        
        
        public string EMail { get; set; }
       
        public string Ad { get; set; }
        
        public string Soyad { get; set; }
        
        public string Parola { get; set; }
        [StringLength(maximumLength: 11, MinimumLength = 10, ErrorMessage = "Telefon Numaranız 10 ya da 11 haneli olmalıdır.")]
        [Phone(ErrorMessage = "Telefon numarasını doğru girdiğinizden emin olunuz!")]
        public string Iletisim { get; set; }
        public string Kullanici { get; set; }
        public string UyelikBaslangicTarihi { get; set; }
        public string UyelikBitisTarihi { get; set; }
        public string ApiUrl { get; set; }
    }
}

