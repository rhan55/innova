using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YKPortal.Models.Dto
{
    public class AnaSayfaDto
    {
        private int _Cari;
        private int _Stok;
        private int _Belge;
        private int _Gorev;

        public int Cari { get { return _Cari == null ? 0 : _Cari; } set { _Cari = value; } }
        public int Stok { get { return _Stok == null ? 0 : _Stok; } set { _Stok = value; } }
        public int Belge { get { return _Belge == null ? 0 : _Belge; } set { _Belge = value; } }
        public int Gorev { get { return _Gorev == null ? 0 : _Gorev; } set { _Gorev = value; } }

    }
}