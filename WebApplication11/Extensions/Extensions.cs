using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YKPortal.Extensions
{
    static public class Extensions
    {
        public static Tarayici TasayiciBilgisiGetir(this HtmlHelper htmlhelper, string tarayiciBilgisi)
        {
            if (tarayiciBilgisi.ToLower().Contains("edg"))
            {
                return Tarayici.Edge;
            }
            else if (tarayiciBilgisi.ToLower().Contains("firefox"))
            {
                return Tarayici.Firefox;
            }
            else if (tarayiciBilgisi.ToLower().Contains("safari"))
            {
                return Tarayici.Safari;
            }
            else if (tarayiciBilgisi.ToLower().Contains("chrome"))
            {
                return Tarayici.Chrome;
            }
            else
            {
                return Tarayici.Diger;
            }
        }
    }

    public enum Tarayici
    {
        Chrome,
        Firefox,
        Safari,
        Edge,
        Diger
    }
}