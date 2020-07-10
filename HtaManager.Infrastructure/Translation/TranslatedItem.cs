using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Translation
{
    public class TranslatedItem
    {
        [JsonProperty("detected_source_language")]
        public string SourceLanguage { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
