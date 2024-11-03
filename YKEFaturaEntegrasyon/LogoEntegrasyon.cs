using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
                            //    //Console.WriteLine(JsonSerializer.Serialize(item));
                            //    // Bilgileri donusturup Cari EFatura bilgilerine ekleyecegim.
                            //}
                        }
                    }
                }
            }
            return results;
        }
    }
}
