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

        public static string Base64DosyaUzantisiniGetir(this string base64String)
        {
            try
            {
                if (base64String.Contains("data:"))
                {
                    string mimeType = base64String.Split(';')[0].Replace("data:", "");

                    // Convert MIME type to file extension

                    string fileExtension = string.Empty;

                    switch (mimeType)
                    {
                        case "image/jpeg":
                            fileExtension = ".jpg";
                            break;
                        case "image/png":
                            fileExtension = ".png";
                            break;
                        case "image/gif":
                            fileExtension = ".gif";
                            break;
                        case "image/bmp":
                            fileExtension = ".bmp";
                            break;
                        case "image/webp":
                            fileExtension = ".webp";
                            break;
                        case "image/svg+xml":
                            fileExtension = ".svg";
                            break;
                        case "application/pdf":
                            fileExtension = ".pdf";
                            break;
                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                            fileExtension = ".docx";
                            break;
                        case "application/msword":
                            fileExtension = ".doc";
                            break;
                        case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                            fileExtension = ".xlsx";
                            break;
                        case "application/vnd.ms-excel":
                            fileExtension = ".xls";
                            break;
                        default:
                            fileExtension = string.Empty; // Default if MIME type is not found
                            break;
                    }

                    return fileExtension;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting file extension: " + ex.Message);
                return string.Empty;
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