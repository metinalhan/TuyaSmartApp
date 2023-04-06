using Newtonsoft.Json;

namespace TuyaApp.Application.Dtos
{
    public class RestResultDto
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }
}
