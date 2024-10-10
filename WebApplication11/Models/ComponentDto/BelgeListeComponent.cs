using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using YKPortal.Models.Dto;

namespace YKPortal.Models.ComponentDto
{
    public class BelgeListeComponent
    {
        public bool TarihVarMi { get; set; } = true;  // Tarih sütunu var mı?
        public bool BelgeNoVarMi { get; set; } = true;  // Belge No sütunu var mı?
        public bool CariIsimVarMi { get; set; } = true;  // Cari sütunu var mı?
        public bool CikisDepoVarMi { get; set; } = true;  // Çıkış depo sütunu var mı?
        public bool GirisDepoVarMi { get; set; } = true;  // Giriş depo sütunu var mı?
        public bool TutarVarMi { get; set; } = true;  // Tutar sütunu var mı?
        public bool IslemVarMi { get; set; } = true;  // İşlem sütunu (Düzenle, Sil) var mı?
        public bool BaslangicTarihiVarMi { get; set; } = true;
        public bool BitisTarihiVarMi { get; set; } = true;
        public bool DurumuVarMi { get; set; } = true;
        public bool SilYetkisiVarMi { get; set; } = true;
        public bool DuzenleYetkisiVarMi { get; set; } = true;
        public string Tip { get; set; }
        public string Baslik { get; set; }
        public DataTable Belgeler { get; set; }  // Listelenen belgeler


    }

}