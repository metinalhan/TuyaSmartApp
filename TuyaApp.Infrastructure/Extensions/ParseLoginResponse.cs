using Newtonsoft.Json.Linq;
using RestSharp;

namespace TuyaApp.Infrastructure.Extensions
{
    public static class ParseLoginResponse
    {
        public static string TokenResponse(this IRestResponse response)
        {
            var jObject = JObject.Parse(response.Content);
            var dataJson = JObject.Parse(jObject["result"].ToString());
            var token = dataJson["access_token"].ToString();

            return token;
        }
    }
}
