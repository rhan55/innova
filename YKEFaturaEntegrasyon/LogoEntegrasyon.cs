using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using YKEFaturaEntegrasyon.EFaturaEDM;
using YKEFaturaEntegrasyon.LogoPostBoxService;

namespace YKEFaturaEntegrasyon
{
    public class LogoEntegrasyon
    {
        private string Username { get; set; }
        private string Password { get; set; }

        public LogoEntegrasyon(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public GibUserInfoType[] CariMukellefKontrolu(string vknTckn)
        {
            GibUserInfoType[] results = null;

            //PostBoxServiceEndpoint bu bilgi sabit text olarak veriyoruz değiştirmeyelim.
            using (PostBoxServiceClient svc = new PostBoxServiceClient("PostBoxServiceEndpoint"))
            {
                string sessionId = "";
                if (svc.Login(new LoginType { userName = Username, passWord = Password }, out sessionId))
                {
                    GibUserType[] userTypes = null;
                    var result = svc.CheckGibUser(sessionId, new string[1] { vknTckn }, out userTypes);

                    if (userTypes != null && userTypes.Length > 0)
                    {
                        if (userTypes[0].InvoiceGbList.Length > 0)
                        {
                            results = userTypes[0].InvoiceGbList;
                            //foreach (var item in userTypes[0].InvoiceGbList)
                            //{
                              //  Console.WriteLine(JsonSerializer.Serialize(item));
                                //    // Bilgileri donusturup Cari EFatura bilgilerine ekleyecegim.
                            //}
                        }
                    }
                }
            }
            return results;
        }



        //public void SendDocumentExample(string userName, string passWord, byte[] fileData, string fileName)
        //    {
        //        using (PostBoxServiceClient svc = new PostBoxServiceClient("PostBoxServiceEndpoint"))
        //        {
        //            if (svc.Login(new LoginType { userName = userName, passWord = passWord }, out string sessionId))
        //            {
        //                string[] paramList = new string[]
        //                {
        //                "DOCUMENTTYPE=DESPATCHADVICE",
        //                "ALIAS=urn:mail:defaulgb@firma.com.tr",
        //                "SIGNED=0"
        //                };

        //                DocumentType document = new DocumentType
        //                {
        //                    binaryData = new base64BinaryData
        //                    {
        //                        Value = fileData
        //                    },
        //                    fileName = fileName,
        //                    hash = HelperLib.ComputeMD5Hash(fileData)
        //                };

        //                ResultType result = svc.SendDocument(sessionId, paramList, document, out int refId);

        //                if (result.resultCode == 1)
        //                    Console.WriteLine($"Gönderildi. Mesaj: {result.resultMsg}, Referans ID: {refId}");
        //                else
        //                    Console.WriteLine($"Gönderilemedi. Hata: {result.resultMsg}");
        //            }
        //            else
        //            {
        //                Console.WriteLine("Login başarısız.");
        //            }
        //        }
        //    }



        //    public GibUserInfoType[] BelgeGonder(string vknTckn)
        //        {
        //            //SendDocument
        //            //Belge gönderimi yapılan metottur.Belge verisi zip formatında sıkıştırılmış olmalıdır.Bir zip dosya içinde birden fazla belge olabilir.
        //            using (PostBoxServiceClient svc = new PostBoxServiceClient("PostBoxServiceEndpoint"))
        //            {
        //                LoginType login = new LoginType
        //                {
        //                    userName = "test",
        //                    passWord = "test",
        //                };

        //                if (svc.Login(login, out string sessionId))
        //                {
        //                    string[] paramList = new string[] {
        //                            "DOCUMENTTYPE=DESPATCHADVICE",
        //                            "ALIAS=urn:mail:defaulgb@firma.com.tr",
        //                            "SIGNED=0",
        //                        };

        //                    DocumentType document = new DocumentType
        //                    {
        //                        binaryData = new base64BinaryData()
        //                        {
        //                            Value = fileData //zip formatında olmalı 
        //                        },
        //                        fileName = fileName,
        //                        hash = HelperLib.ComputeMD5Hash(fileData),
        //                    };

        //                    ServiceReference1.ResultType result = svc.SendDocument(sessionId, paramList, document, out int refId);

        //                    if (result.resultCode == 1)
        //                        Console.Write("Gönderildi " + result.resultMsg);
        //                    else
        //                        Console.Write("Gönderilemedi " + result.resultMsg);
        //                }
        //            }


        //        }
    }

}
