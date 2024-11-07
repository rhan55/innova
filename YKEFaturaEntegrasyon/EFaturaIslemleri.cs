using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YKEFaturaEntegrasyon.Dto;

namespace YKEFaturaEntegrasyon
{
    public class EFaturaIslemleri
    { 
        public static EFaturaAyarlariDto EFaturaLogoPostBoxServiceAyarlariGetir()
        {
            //Bu bilgiler veritabanından UyelikID'si ile getirilecek.

            EFaturaAyarlariDto entity = new EFaturaAyarlariDto()
            {
                KullaniciAdi = "9811613622",
                Sifre = "1986*1986rY",
            };
            return entity;
        }
    }
}
