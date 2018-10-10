using Newtonsoft.Json.Serialization;

namespace Functions.Infrastructure
{
    public class DefaultJsonSerializerSettings : Newtonsoft.Json.JsonSerializerSettings
    {
        public DefaultJsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver();
            Formatting = Newtonsoft.Json.Formatting.Indented;
            DateFormatString = "yyyy-MM-ddTHH:mm:ssZ";
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        }
    }
}
