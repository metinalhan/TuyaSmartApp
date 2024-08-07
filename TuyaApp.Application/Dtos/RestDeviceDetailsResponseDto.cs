using Newtonsoft.Json;

namespace TuyaApp.Application.Dtos
{
    public class RestDeviceDetailsResponseDto
    {
        [JsonProperty("result")]
        public RestDeviceDetailsResultDto Result { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("t")]
        public long T { get; set; }
    }
}
