using RestSharp;

namespace TuyaApp.Infrastructure.Helpers
{
    public static class RestRequestBuilder
    {
        // This method creates a GET RestRequest object with the specified headers and parameters.
        public static RestRequest GetRestRequest(string clientId, string hash, string t, string token = null)
        {
            // Create a new RestRequest object with the GET HTTP method.
            var request = new RestRequest() { Method = Method.Get};

            // Add the client ID header to the request.
            request.AddHeader("client_id", clientId);

            // If a token is provided, add the access token header to the request.
            if (token is not null)
                request.AddHeader("access_token", token);

            // Add the sign, t, sign_method, and Content-Type headers to the request.
            request.AddHeader("sign", hash);
            request.AddHeader("t", t);
            request.AddHeader("sign_method", "HMAC-SHA256");
            request.AddHeader("Content-Type", "application/json");

            // Return the RestRequest object.
            return request;
                      
                     
        }

        // This method creates a POST RestRequest object with the specified headers and parameters.
        public static RestRequest PostRestRequest(string clientId, string hash, string t,string token)
        {
            // Create a new RestRequest object with the POST HTTP method.
            var request = new RestRequest() { Method = Method.Post }; ;

            // Add the client ID, access token, sign, t, sign_method, and Content-Type headers to the request.
            request.AddHeader("client_id", clientId);
            request.AddHeader("access_token", token);
            request.AddHeader("sign", hash);
            request.AddHeader("t", t);
            request.AddHeader("sign_method", "HMAC-SHA256");
            request.AddHeader("Content-Type", "application/json");

            // Return the RestRequest object.
            return request;
        }
    }
}
