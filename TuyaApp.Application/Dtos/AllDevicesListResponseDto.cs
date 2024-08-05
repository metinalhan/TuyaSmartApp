using System.Collections.Generic;
using Newtonsoft.Json;

namespace TuyaApp.Application.Dtos
{
    public class AllDevicesListResponseDto
    {
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("id")]
        public string DeviceId { get; set; }

        [JsonProperty("name")]
        public string DeviceName { get; set; }

        [JsonProperty("status")]
        public List<RestResultDto> DeviceSpecs { get; set; }
    }
}
