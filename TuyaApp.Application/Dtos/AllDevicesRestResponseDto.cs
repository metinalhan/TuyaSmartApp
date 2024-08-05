using System.Collections.Generic;
using Newtonsoft.Json;

namespace TuyaApp.Application.Dtos
{
    public class DevicesRestResponse
    {
        [JsonProperty("devices")]
        public List<AllDevicesListResponseDto> UserDevicesResult { get; set; }
    }

    public class AllDevicesRestResponseDto
    {
        [JsonProperty("result")]
        public DevicesRestResponse Devices { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("t")]
        public long T { get; set; }
    }
}
