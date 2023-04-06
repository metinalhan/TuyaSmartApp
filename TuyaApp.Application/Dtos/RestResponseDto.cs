using Newtonsoft.Json;
using System.Collections.Generic;

namespace TuyaApp.Application.Dtos
{
    public class RestResponseDto
    {
        [JsonProperty("result")]
        public List<RestResultDto> Result { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("t")]
        public long T { get; set; }
    }
}
