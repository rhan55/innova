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
        public static EFaturaLogoPostBoxServiceDto EFaturaLogoPostBoxServiceAyarlariGetir()
        {
            //Bu bilgiler veritabanından UyelikID'si ile getirilecek.

            EFaturaLogoPostBoxServiceDto entity = new EFaturaLogoPostBoxServiceDto()
            {
                EFaturaLogoPostBoxServiceKullaniciAdi = "9811613622",
                EFaturaLogoPostBoxServiceSifre = "1986*1986rY",
            };
            return entity;
        }
    }
}
