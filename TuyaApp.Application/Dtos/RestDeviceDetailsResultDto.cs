using Newtonsoft.Json;

namespace TuyaApp.Application.Dtos
{
    public class RestDeviceDetailsResultDto
    {
        [JsonProperty("online")]
        public string IsOnline { get; set; }

        [JsonProperty("update_time")]
        public long UpdateTime { get; set; }
    }
}
