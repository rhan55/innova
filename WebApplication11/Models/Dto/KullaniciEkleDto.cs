using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication11.Models.Dto
{
    public class KullaniciEkleDto
    {
        public string ID { get; set; }
        public string UyelikID { get; set; }
        [Required(ErrorMessage = "Kullanıcı Adını Giriniz.")]
        public string KullaniciAdi { get; set; }
        [Required(ErrorMessage= "Kullanıcı Parolasını Giriniz")]
        public string Parola { get; set; }
        [Required(ErrorMessage = "Ad Giriniz")]
        public string Ad { get; set; }
        [Required(ErrorMessage = "Soyad Giriniz")]
        public string Soyad { get; set; }
        public bool Aktif { get; set; }
        public string Telefon { get; set; }
        public string Adres { get; set; }
        public string Il { get; set; }
        public string Ilce { get; set; }
        public string Aciklama1 { get; set; }
        public string Aciklama2 { get; set; }
        public string Aciklama3 { get; set; }
        public string Kullanici { get; set; }
     
    }
}

