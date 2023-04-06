using Newtonsoft.Json;
using RestSharp;
using TuyaApp.Application.Dtos;

namespace TuyaApp.Infrastructure.Extensions
{
    public static class ParseLastStateResponse
    {
        public static RestResponseDto DeserializeResponse(this IRestResponse response) =>        
             JsonConvert.DeserializeObject<RestResponseDto>(response.Content);
        
    }
}
