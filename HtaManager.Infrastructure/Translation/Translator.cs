using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Translation
{
    public static class Translator
    {
        public static string Translate(string Text)
        {
            if (string.IsNullOrEmpty(Text)) return string.Empty;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string result = string.Empty;
            string url = $"https://api.deepl.com/v2/translate?auth_key=a30d9f41-a4f6-96c2-3e69-6fc676313dae&text={Text}&source_lang=EN&target_lang=DE&formality=more";

            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers.Add("user-agent", " Mozilla/5.0 (Windows NT 6.1; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0");
                result = webClient.DownloadString(url);
            }

            if (!string.IsNullOrEmpty(result))
            {
                JObject jObj = JObject.Parse(result);
                var token = jObj.SelectToken("translations");
                string jsonString = JsonConvert.SerializeObject(token);

                List<TranslatedItem> translationList = JsonConvert.DeserializeObject<List<TranslatedItem>>(jsonString);

                if (translationList.Count > 0)
                    return translationList[0].Text;
                else
                    return Text;
            }
            else
            {
                return Text;
            }
        }
    }
}