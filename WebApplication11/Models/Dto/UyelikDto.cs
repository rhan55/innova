using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication11.Models.Dto
{
    public class UyelikDto
    {
        public string ID { get; set; }
        [Required(ErrorMessage = "Firma Kısa Adını Giriniz")]
        public string Isim { get; set; }
        [Required(ErrorMessage = "Firma Tam Ünvanını Giriniz")]
        public string Unvan { get; set; }
        [StringLength(maximumLength:11, MinimumLength = 10, ErrorMessage = "Vergi Numarası 10 ya da 11 haneli olmalıdır.")]
        [Required(ErrorMessage = "Vergi numarası giriniz!")]
        public string VergiNumarasi { get; set; }
        [Required(ErrorMessage = "Vergi dairesi giriniz!")]
        public string VergiDairesi { get; set; }
        [Required(ErrorMessage = "Adres giriniz!")]
        public string Adres { get; set; }
        [Required(ErrorMessage = "İl Giriniz!")]
        public string Il { get; set; }
        [Required(ErrorMessage = "İlçe Giriniz!")]
        public string Ilce { get; set; }
        [Required(ErrorMessage = "Kullanıcı adı/E-Mail giriniz!")]
        [EmailAddress(ErrorMessage = "Mail adresi olmalıdır!")]
        public string EMail { get; set; }
        [Required(ErrorMessage = "Ad giriniz!")]
        public string Ad { get; set; }
        [Required(ErrorMessage = "Soyad giriniz!")]
        public string Soyad { get; set; }
        [Required(ErrorMessage = "Kullanıcı parolasını giriniz!")]
        public string Parola { get; set; }
        [Required(ErrorMessage = "İletişim Numarası giriniz")]
        [Phone(ErrorMessage = "Telefon numarasını doğru girdiğinizden emin olunuz!")]
        public string Iletisim { get; set; }
        public string Kullanici { get; set; }
        public string UyelikBaslangicTarihi { get; set; }
        public string UyelikBitisTarihi { get; set; }
        public string ApiUrl { get; set; }
    }
}

