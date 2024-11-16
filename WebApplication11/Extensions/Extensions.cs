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
            else if (tarayiciBilgisi.ToLower().Contains("safari") && !tarayiciBilgisi.ToLower().Contains("chrome"))
            {
                return Tarayici.Safari;
            }
            else if (tarayiciBilgisi.ToLower().Contains("chrome") && !tarayiciBilgisi.ToLower().Contains("edg") && !tarayiciBilgisi.ToLower().Contains("opr"))
            {
                return Tarayici.Chrome;
            } else if (tarayiciBilgisi.ToLower().Contains("opr") || tarayiciBilgisi.ToLower().Contains("opera"))
            {
                return Tarayici.Opera;
            } else if (tarayiciBilgisi.ToLower().Contains("Brave"))
            {
                return Tarayici.Brave;
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
        Opera,
        Brave,
        Diger
    }
}