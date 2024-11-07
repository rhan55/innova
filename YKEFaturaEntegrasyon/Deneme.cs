using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YKEFaturaEntegrasyon.Dto;

namespace YKEFaturaEntegrasyon
{
    public partial class Deneme : Form
    {
        public Deneme()
        {
            InitializeComponent();
        }

        private void Deneme_Load(object sender, EventArgs e)
        {
            try
            {
              
                EFaturaAyarlariDto eFaturaLogoAyarlari = YKEFaturaEntegrasyon.EFaturaIslemleri.EFaturaLogoPostBoxServiceAyarlariGetir();

                var logoEntegrasyon = new LogoEntegrasyon(
                        eFaturaLogoAyarlari.KullaniciAdi,
                        eFaturaLogoAyarlari.Sifre
                    );

                logoEntegrasyon.CariMukellefKontrolu("9811613622");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
